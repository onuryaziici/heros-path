using UnityEngine;
using UnityEngine.AI; // NavMeshAgent için

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f; // Oyuncuyu algılama mesafesi
    public float attackRange = 1.5f;   // Oyuncuya saldırı yapma mesafesi (temas)
    public float moveSpeed = 3f;

    public float damageOnContact = 5f;       // Temas halinde oyuncuya verilecek hasar
    public float attackCooldown = 2f;        // Temasla hasar verme sıklığı
    private float lastAttackTime_Enemy = -Mathf.Infinity;

    private Transform playerTarget;
    private NavMeshAgent agent; // NavMeshAgent kullanacağız
    // private Animator animator; // Animasyon için

    void Start()
    {
        // Oyuncuyu bul (Player etiketine sahip olmalı)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogError("Sahnede 'Player' etiketli bir obje bulunamadı!");
            enabled = false; // Bu script'i devre dışı bırak
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>(); // Eğer yoksa ekle
        }
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange * 0.8f; // Saldırı menzilinin biraz içinde dursun

        // animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Oyuncuya doğru hareket et
            agent.SetDestination(playerTarget.position);
            // animator.SetBool("IsWalking", true); // Yürüme animasyonu (daha sonra)

            // Oyuncu saldırı menzilindeyse ve cooldown süresi dolduysa hasar ver
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime_Enemy + attackCooldown)
            {
                PlayerHealth playerHealth = playerTarget.GetComponent<PlayerHealth>(); // PlayerHealth script'ine ihtiyacımız olacak
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageOnContact);
                    lastAttackTime_Enemy = Time.time;
                    Debug.Log("Düşman oyuncuya temasla hasar verdi!");
                }
                // animator.SetTrigger("Attack"); // Düşman saldırı animasyonu (daha sonra)
            }
        }
        // else
        // {
        //     animator.SetBool("IsWalking", false); // Durma animasyonu (daha sonra)
        // }
    }

    // Algılama menzilini Scene view'da görmek için (isteğe bağlı)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}