// EnemyAI.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Coroutine için gerekli

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float moveSpeed = 3f;

    [Header("Attack Settings")]
    public float damageAmount = 5f;
    public float attackCooldown = 2f;
    
    [Header("State Durations")]
    [Tooltip("Saldırı animasyonunun yaklaşık uzunluğu.")]
    public float attackAnimationDuration = 1.2f;
    [Tooltip("Hasar alma animasyonunun süresi.")]
    public float hurtAnimationDuration = 0.5f;

    [Header("Combat References")]
    public GameObject attackHitboxObject;

    // --- Private Değişkenler ---
    private Transform playerTarget;
    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking = false;
    private bool isTakingDamage = false;

    // Animator parametre ID'leri
    private readonly int isWalkingParam = Animator.StringToHash("IsWalking");
    private readonly int attackParam = Animator.StringToHash("Attack");
    private readonly int hurtParam = Animator.StringToHash("Hurt"); // Hurt trigger'ı için

    void Start()
    {
        // ... (Start fonksiyonunun referans alma kısmı aynı)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) playerTarget = playerObject.transform;
        else { enabled = false; return; }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) { enabled = false; return; }
        
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogWarning("Animator not found!");

        if (attackHitboxObject != null) attackHitboxObject.SetActive(false);
        else Debug.LogWarning("Enemy attackHitboxObject not assigned.");

        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange * 0.9f;
    }

    void Update()
    {
        if (playerTarget == null) return;

        // Düşman sadece meşgul değilken (saldırmıyor veya hasar almıyorken) hareket mantığını çalıştırır.
        if (!isAttacking && !isTakingDamage)
        {
            HandleMovementAndDetection();
        }
    }

    void HandleMovementAndDetection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > agent.stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTarget.position);
                if (animator != null) animator.SetBool(isWalkingParam, true);
            }
            else
            {
                agent.isStopped = true;
                if (animator != null) animator.SetBool(isWalkingParam, false);
                FaceTarget();
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    StartAttack();
                }
            }
        }
        else
        {
            agent.isStopped = true;
            if (animator != null) animator.SetBool(isWalkingParam, false);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed / 100);
        }
    }

    void StartAttack()
    {
        if (isAttacking || isTakingDamage) return;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        agent.isStopped = true; // Saldırı sırasında hareket etme

        if (animator != null)
        {
            animator.SetTrigger(attackParam);
        }

        yield return new WaitForSeconds(attackAnimationDuration);
        isAttacking = false;
    }

    // Bu fonksiyon EnemyHealth tarafından çağrılır
    public void TriggerHurtState()
    {
        if (!isTakingDamage)
        {
            StartCoroutine(HurtStateCoroutine());
        }
    }

    IEnumerator HurtStateCoroutine()
    {
        isTakingDamage = true;
        agent.isStopped = true; // Hasar alırken hareket etme
        if(animator != null) animator.SetBool(isWalkingParam, false);

        yield return new WaitForSeconds(hurtAnimationDuration);
        isTakingDamage = false;
    }

    // --- Animation Event Functions ---

    // Bu public fonksiyon, Goblin'in saldırı animasyonundaki bir Event tarafından çağrılacak.
    public void PlayAttackSound()
    {
        // AudioManager'a Goblin'in pozisyonunda bir saldırı sesi çalmasını söyleyebiliriz.
        // Veya genel, pozisyonsuz bir ses de olabilir. Şimdilik pozisyonsuz yapalım.
        // Eğer AudioManager'da "enemyAttack" gibi bir klip varsa:
        // AudioManager.instance.PlayEnemyAttackSound(); 
        
        // Eğer genel bir "swoosh" sesi kullanacaksak:
        AudioManager.instance.PlayGoblinAttack(transform.position); // Geçici olarak oyuncunun sesini kullanabiliriz.
    }
    public void EnableHitbox()
    {
        if (attackHitboxObject != null)
        {
            attackHitboxObject.SetActive(true);
        }
    }

    public void DisableHitbox()
    {
        if (attackHitboxObject != null)
        {
            attackHitboxObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}