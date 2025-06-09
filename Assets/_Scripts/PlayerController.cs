// PlayerController.cs
using UnityEngine;
using System.Collections; 

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;            // Karakterin hareket hızı
    public float rotationSpeed = 720f;      // Karakterin dönüş hızı (derece/saniye)

    [Header("Combat")]
    public float attackCooldown = 1f;       // Saldırılar arası bekleme süresi
    public GameObject attackHitboxObject;   // Saldırı anında aktif olacak hitbox objesi
    public float attackActiveTime = 0.2f;   // Hitbox'ın ne kadar süre aktif kalacağı

    // --- Private Değişkenler ---
    private CharacterController characterController; // Fiziksel hareket ve çarpışmalar için
    private Camera mainCamera;                       // Oyuncunun hareket yönünü belirlemek için ana kamera
    private Animator animator;                       // Animasyonları kontrol etmek için
    private float lastAttackTime = -Mathf.Infinity;  // Saldırı cooldown'unu hesaplamak için

    void Start()
    {
        // Gerekli bileşenleri başlangıçta alıp referanslarını sakla (performans için iyidir)
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        
        // Animator genellikle modelin olduğu child objede olabilir, bu yüzden GetComponentInChildren daha güvenlidir.
        animator = GetComponentInChildren<Animator>();

        // Başlangıçta referansların alınıp alınamadığını kontrol et
        if (characterController == null)
            Debug.LogError("Player üzerinde CharacterController bileşeni bulunamadı!");
        if (mainCamera == null)
            Debug.LogError("Sahnede 'MainCamera' etiketli bir kamera bulunamadı!");
        if (animator == null)
            Debug.LogWarning("Player üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        if (attackHitboxObject != null)
            attackHitboxObject.SetActive(false); // Oyun başında hitbox'ın kapalı olduğundan emin ol
        else
            Debug.LogError("Attack Hitbox Object PlayerController'a atanmamış!");
    }

    void Update()
    {
        // Oyunun ana döngüsünde her karede bu fonksiyonları çağır
        HandleMovement();
        HandleAttack();
    }

    // Hareket ve rotasyon mantığını yöneten fonksiyon
    void HandleMovement()
    {
        // Kullanıcıdan WASD veya Ok tuşları ile input al (değerler -1 ile 1 arasında olur)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Kameranın ileri ve sağ yönlerini al
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // Kameranın dikey eğiminden (aşağı bakmasından) etkilenmemek için Y eksenini sıfırla
        forward.y = 0;
        right.y = 0;
        forward.Normalize(); // Vektörleri birim vektör (uzunluğu 1) yap
        right.Normalize();

        // Input'a ve kameranın yönüne göre hedeflenen hareket yönünü hesapla
        Vector3 desiredMoveDirection = forward * verticalInput + right * horizontalInput;

        // Karakteri CharacterController ile hareket ettir
        characterController.Move(desiredMoveDirection.normalized * moveSpeed * Time.deltaTime);

        // Animator'u güncelle
        if (animator != null)
        {
            // CharacterController'ın mevcut hızının büyüklüğünü al.
            // Bu, karakterin ne kadar hızlı hareket ettiğini gösterir (0 ise duruyor demektir).
            float currentSpeed = characterController.velocity.magnitude;
            animator.SetFloat("MoveSpeed", currentSpeed);
        }

        // Karakteri hareket yönüne doğru döndür
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Saldırı input'unu dinleyen fonksiyon
    void HandleAttack()
    {
        // "Fire1" (genellikle Sol Fare Tıklaması) basıldıysa VE saldırı bekleme süresi dolduysa...
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }
    }

    // Saldırı mantığını başlatan fonksiyon
    void PerformAttack()
    {
        lastAttackTime = Time.time; // Son saldırı zamanını şimdi olarak ayarla

        // Animator'e saldırı animasyonunu tetiklemesini söyle
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Saldırı hitbox'ını belirli bir süre için aktif et
        if (attackHitboxObject != null)
        {
            StartCoroutine(ActivateHitbox());
        }
    }

    // Hitbox'ı belirli bir süre aktif edip sonra kapatan Coroutine
    IEnumerator ActivateHitbox()
    {
        attackHitboxObject.SetActive(true);
        yield return new WaitForSeconds(attackActiveTime);
        attackHitboxObject.SetActive(false);
    }
}