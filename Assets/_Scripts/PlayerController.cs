// PlayerController.cs
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Combat")]
    public float attackCooldown = 1f;
    public GameObject attackHitboxObject;
    public float attackActiveTime = 0.2f;
    [Tooltip("Saldırı animasyonunun yaklaşık uzunluğu. Bu süre boyunca hareket engellenir.")]
    public float attackAnimationDuration = 0.5f;
    [Tooltip("Hasar alma animasyonunun süresi. Bu süre boyunca hareket ve saldırı engellenir.")]
    public float hurtAnimationDuration = 0.4f;

    // --- Private Değişkenler ---
    private CharacterController characterController;
    private Camera mainCamera;
    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking = false;
    private bool isTakingDamage = false; // YENİ: Hasar alma durumunu takip eden bayrak

    // Animator parametre ID'leri
    private readonly int moveSpeedParam = Animator.StringToHash("MoveSpeed");
    private readonly int attackParam = Animator.StringToHash("Attack");
    private readonly int hurtParam = Animator.StringToHash("Hurt"); // YENİ: Hurt parametresi

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();

        if (characterController == null)
            Debug.LogError("Player üzerinde CharacterController bileşeni bulunamadı!");
        if (mainCamera == null)
            Debug.LogError("Sahnede 'MainCamera' etiketli bir kamera bulunamadı!");
        if (animator == null)
            Debug.LogWarning("Player üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        if (attackHitboxObject != null)
            attackHitboxObject.SetActive(false);
        else
            Debug.LogError("Attack Hitbox Object PlayerController'a atanmamış!");
    }

    void Update()
    {
        // Saldırı veya hasar alma sırasında hareketi engelle
        if (!isAttacking && !isTakingDamage)
        {
            HandleMovement();
        }
        
        HandleAttack();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * verticalInput + right * horizontalInput).normalized;

        characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            float currentSpeed = characterController.velocity.magnitude;
            animator.SetFloat(moveSpeedParam, currentSpeed);
        }

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleAttack()
    {
        // Saldırı bekleme süresi dolduysa VE şu anda meşgul değilse (saldırmıyor veya hasar almıyorsa)...
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown && !isAttacking && !isTakingDamage)
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger(attackParam);
        }
        
        StartCoroutine(AttackCommitment());
    }

    // YENİ FONKSİYON: PlayerHealth tarafından çağrılacak
    public void TriggerHurtState()
    {
        if (!isTakingDamage)
        {
            if (animator != null)
            {
                animator.SetTrigger(hurtParam);
            }
            StartCoroutine(HurtStateCoroutine());
        }
    }

    public void ActivateDamageHitbox()
    {
        if (attackHitboxObject != null)
        {
            StartCoroutine(ActivateHitboxCoroutine());
        }
    }

    IEnumerator ActivateHitboxCoroutine()
    {
        attackHitboxObject.SetActive(true);
        yield return new WaitForSeconds(attackActiveTime);
        attackHitboxObject.SetActive(false);
    }
    
    IEnumerator AttackCommitment()
    {
        yield return new WaitForSeconds(attackAnimationDuration);
        isAttacking = false;
    }

    // YENİ COROUTINE: Hasar alma durumunu yönetir
    IEnumerator HurtStateCoroutine()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(hurtAnimationDuration);
        isTakingDamage = false;
    }
}