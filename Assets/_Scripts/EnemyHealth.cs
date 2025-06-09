// EnemyHealth.cs
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // UI elemanları (Slider) için bu satır gerekli

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 30f;
    [SerializeField] // currentHealth'i Inspector'da görmek için ama diğer script'lerden değiştirilmesini engellemek için
    private float currentHealth;

    [Header("UI")]
    public Slider healthBarSlider;          // Düşmanın üzerindeki can barı Slider'ı
    public Canvas healthBarCanvasComponent; // Can barının Canvas bileşeni

    [Header("Death")]
    public float timeBeforeDestroy = 2.5f; // Ölüm animasyonundan sonra ne kadar bekleyip yok olacağı

    // --- Private Değişkenler ---
    private Animator animator;     // Animator bileşenini referans almak için
    private bool isDead = false;   // Ölüm fonksiyonunun birden fazla kez çağrılmasını engellemek için

    void Awake()
    {
        // Animator bileşenini al (Genellikle model bir child obje olduğundan GetComponentInChildren kullanmak daha güvenlidir)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning(gameObject.name + " üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        }

        // Eğer Inspector'dan atanmadıysa, child objelerden Canvas'ı bulmayı dene
        if (healthBarCanvasComponent == null)
        {
            healthBarCanvasComponent = GetComponentInChildren<Canvas>();
        }

        if (healthBarCanvasComponent != null)
        {
            // World Space Canvas'ın kamerasını kodla ata
            if (healthBarCanvasComponent.renderMode == RenderMode.WorldSpace)
            {
                healthBarCanvasComponent.worldCamera = Camera.main;
            }
        }
        else
        {
            Debug.LogWarning("EnemyHealth: Health Bar Canvas component not found on " + gameObject.name);
        }
        
        // Eğer Inspector'dan atanmadıysa, Canvas'ın altından Slider'ı bulmayı dene
        if (healthBarSlider == null && healthBarCanvasComponent != null)
        {
            healthBarSlider = healthBarCanvasComponent.GetComponentInChildren<Slider>();
        }
    }

    void Start()
    {
        // Oyuna başlarken canı fulle
        currentHealth = maxHealth;

        // Can barını başlangıç değerlerine ayarla
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    // Hasar alma fonksiyonu. Diğer script'ler (örn: AttackHitbox) bu fonksiyonu çağıracak.
    public void TakeDamage(float amount)
    {
        // Eğer düşman zaten öldüyse, tekrar hasar almasını engelle
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Can barını güncelle
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        // Hasar alma animasyonunu tetikle (eğer varsa)
        if (animator != null && currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
        }

        // Eğer can sıfıra veya altına indiyse, ölüm fonksiyonunu çağır
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Ölüm mantığını içeren fonksiyon
    void Die()
    {
        // Bu fonksiyonun birden fazla kez çalışmasını engelle
        if (isDead) return;
        isDead = true;

        Debug.Log(gameObject.name + " öldü!");

        // Ölüm animasyonunu tetikle
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Can barını gizle
        if (healthBarCanvasComponent != null)
        {
            healthBarCanvasComponent.gameObject.SetActive(false);
        }

        // Düşmanın çarpışmasını ve hareket etmesini engellemek için bileşenlerini devre dışı bırak.
        // Bu, ölüm animasyonu oynarken başka şeylerin ona çarpmasını veya onun hareket etmesini engeller.
        Collider mainCollider = GetComponent<Collider>();
        if (mainCollider != null) mainCollider.enabled = false;
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        
        EnemyAI aiScript = GetComponent<EnemyAI>();
        if(aiScript != null) aiScript.enabled = false;

        // Ölüm animasyonunun oynaması için bir süre bekledikten sonra objeyi sahneden tamamen sil
        Destroy(gameObject, timeBeforeDestroy);
    }
    
    // Can barının her zaman kameraya doğru bakmasını sağlayan fonksiyon
    void LateUpdate()
    {
        if (isDead) return; // Öldüyse can barını döndürmeye gerek yok

        if (healthBarCanvasComponent != null && healthBarCanvasComponent.gameObject.activeInHierarchy && healthBarCanvasComponent.worldCamera != null)
        {
            Transform cameraTransform = healthBarCanvasComponent.worldCamera.transform;
            healthBarCanvasComponent.transform.LookAt(healthBarCanvasComponent.transform.position + cameraTransform.rotation * Vector3.forward,
                                                     cameraTransform.rotation * Vector3.up);
        }
    }
}