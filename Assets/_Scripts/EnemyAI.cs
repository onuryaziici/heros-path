// EnemyAI.cs
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent kullanmak için bu satır gerekli

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;  // Oyuncuyu ne kadar uzaktan fark edeceği
    public float attackRange = 1.5f;    // Oyuncuya ne kadar yaklaşınca saldıracağı
    public float moveSpeed = 3f;        // Hareket hızı

    [Header("Attack Settings")]
    public float damageOnContact = 5f;  // Temas halinde oyuncuya verilecek hasar
    public float attackCooldown = 2f;   // Ne sıklıkla saldırabileceği (saniye)

    // --- Private Değişkenler ---
    private Transform playerTarget;             // Takip edilecek oyuncunun Transform'u
    private NavMeshAgent agent;                 // Hareket için NavMeshAgent bileşeni
    private Animator animator;                  // Animasyonları kontrol etmek için Animator bileşeni
    private float lastAttackTime = -Mathf.Infinity; // En son ne zaman saldırdığını takip etmek için (cooldown hesaplaması)

    void Start()
    {
        // Oyuncuyu "Player" etiketine göre bul
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            // Eğer sahnede "Player" etiketli bir obje yoksa, bu script hata vermemesi için kendini devre dışı bırakır.
            Debug.LogError("Sahnede 'Player' etiketli bir obje bulunamadı! EnemyAI devre dışı bırakılıyor.");
            enabled = false;
            return;
        }

        // NavMeshAgent bileşenini al
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError(gameObject.name + " üzerinde NavMeshAgent bileşeni bulunamadı!");
            enabled = false;
            return;
        }
        
        // Animator bileşenini al (Genellikle model bir child obje olduğundan GetComponentInChildren kullanmak daha güvenlidir)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning(gameObject.name + " üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        }

        // NavMeshAgent ayarlarını yap
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange * 0.9f; // Saldırı menzilinin biraz içinde durmasını sağlar, böylece tam temas eder.
    }

    void Update()
    {
        // Gerekli referanslar yoksa (oyuncu öldüyse vb.) Update fonksiyonunu çalıştırma
        if (playerTarget == null) return;

        // Oyuncu ile düşman arasındaki mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // Eğer oyuncu algılama menzili içindeyse...
        if (distanceToPlayer <= detectionRange)
        {
            // Eğer oyuncu durma mesafesinin dışındaysa (yani oyuncuya doğru hareket etmesi gerekiyorsa)...
            if (distanceToPlayer > agent.stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTarget.position);
                
                // Animator'e yürüdüğünü bildir
                if (animator != null)
                    animator.SetBool("IsWalking", true);
            }
            // Eğer oyuncu durma mesafesi içindeyse (yani durup saldırması gerekiyorsa)...
            else
            {
                agent.isStopped = true;
                
                // Animator'e durduğunu bildir
                if (animator != null)
                    animator.SetBool("IsWalking", false);

                // Yüzünü oyuncuya doğru döndür
                FaceTarget();

                // Saldırı cooldown süresi dolduysa saldır
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
            }
        }
        // Eğer oyuncu algılama menzili dışındaysa...
        else
        {
            agent.isStopped = true;

            // Animator'e durduğunu bildir
            if (animator != null)
                animator.SetBool("IsWalking", false);
        }
    }

    // Yüzünü hedefe (oyuncuya) doğru yavaşça döndüren fonksiyon
    void FaceTarget()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed / 100);
        }
    }

    // Saldırı mantığını içeren fonksiyon
    void Attack()
    {
        lastAttackTime = Time.time; // Son saldırı zamanını güncelle

        // Animator'e saldırı animasyonunu tetiklemesini söyle
        if (animator != null)
            animator.SetTrigger("Attack");

        // Oyuncunun PlayerHealth script'ini alıp hasar ver
        PlayerHealth playerHealth = playerTarget.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Not: Hasar verme anını animasyonun belirli bir karesine (Animation Event) bağlamak daha isabetli olur.
            // Şimdilik animasyon tetiklenir tetiklenmez hasar veriyoruz.
            playerHealth.TakeDamage(damageOnContact);
            Debug.Log(gameObject.name + " oyuncuya " + damageOnContact + " hasar verdi!");
        }
    }

    // Sahne görünümünde menzilleri görselleştirmek için (hata ayıklamada kullanışlıdır)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}