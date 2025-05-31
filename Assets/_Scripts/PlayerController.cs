// PlayerController.cs (Devamı)
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    public float attackCooldown = 1f; // Saniye cinsinden saldırı bekleme süresi
    private float lastAttackTime = -Mathf.Infinity; // En son ne zaman saldırı yapıldığı

    public GameObject attackHitboxObject;
    public float attackActiveTime = 0.2f; // Hitbox'ın ne kadar süre aktif kalacağı

    private CharacterController characterController;
    private Camera mainCamera;
    // Animator animator; // Animasyon için daha sonra eklenecek

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        if (attackHitboxObject != null)
            attackHitboxObject.SetActive(false);
        else
            Debug.LogError("Attack Hitbox Object PlayerController'a atanmamış!");

        // animator = GetComponentInChildren<Animator>(); // Eğer karakter modeliniz bir child obje ise
    }

    void Update()
    {
        HandleMovement();
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

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        moveDirection.Normalize();

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleAttack()
    {
        // Sol fare tuşuna basıldıysa VE cooldown süresi dolduysa
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown) // "Fire1" genellikle Sol Fare Tıklamasıdır (Input Manager'dan kontrol edilebilir)
        {
            PerformAttack();
            lastAttackTime = Time.time; // Son saldırı zamanını güncelle
        }
    }

    void PerformAttack()
    {
        Debug.Log("Oyuncu Saldırdı!"); // Şimdilik konsola yazdıralım
        // animator.SetTrigger("Attack"); // Animasyon tetikleyicisi (daha sonra)

        // Burada hasar verme mantığı olacak
        if (attackHitboxObject != null)
        {
            StartCoroutine(ActivateHitbox());
        }
    }
    System.Collections.IEnumerator ActivateHitbox()
    {
        attackHitboxObject.SetActive(true);
        yield return new WaitForSeconds(attackActiveTime); // Belirtilen süre kadar bekle
        attackHitboxObject.SetActive(false);
    }
}