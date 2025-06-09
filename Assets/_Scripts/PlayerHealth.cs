// PlayerHealth.cs
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] // currentHealth'i Inspector'da görmek için ama diğer script'lerden değiştirilmesini engellemek için
    private float currentHealth;

    [Header("Dependencies")]
    public UIManager uiManager; // UI güncellemeleri için UIManager referansı

    // --- Private Değişkenler ---
    private Animator animator;     // Animator bileşenini referans almak için
    private bool isDead = false;   // Ölüm fonksiyonunun birden fazla kez çağrılmasını engellemek için

    void Start()
    {
        // Oyuna başlarken canı fulle
        currentHealth = maxHealth;

        // Gerekli referansları al
        animator = GetComponentInChildren<Animator>();

        // Eğer UIManager Inspector'dan atanmadıysa, sahnede bulmayı dene (güvenlik için)
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
                Debug.LogError("Sahnede UIManager bulunamadı!");
        }

        // Başlangıçta can barını güncelle
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        // Referans kontrolü
        if (animator == null)
            Debug.LogWarning("Player üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
    }

    // Hasar alma fonksiyonu. Düşmanlar bu fonksiyonu çağıracak.
    public void TakeDamage(float amount)
    {
        // Eğer oyuncu zaten öldüyse, tekrar hasar almasını engelle
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // UI'daki can barını güncelle
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        // Hasar alma animasyonunu tetikle (eğer varsa ve hala hayattaysa)
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

    // Can yenileme fonksiyonu (iksir vb. için)
    public void Heal(float amount)
    {
        // Ölü oyuncu can yenileyemez
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Oyuncu iyileşti: " + currentHealth);

        // UI'daki can barını güncelle
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
    }

    // Ölüm mantığını içeren fonksiyon
    void Die()
    {
        // Bu fonksiyonun birden fazla kez çalışmasını engelle
        if (isDead) return;
        isDead = true;

        Debug.Log("Oyuncu Öldü! Oyun Bitti.");

        // Ölüm animasyonunu tetikle
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Oyuncunun daha fazla hareket etmesini veya saldırmasını engellemek için kontrol script'ini devre dışı bırak
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;
        
        // Oyuncunun CharacterController'ını devre dışı bırakarak daha fazla hareket veya çarpışmayı engelle
        CharacterController charController = GetComponent<CharacterController>();
        if(charController != null)
            charController.enabled = false;

        // UIManager aracılığıyla "Oyun Bitti" ekranını göster
        if (uiManager != null)
            uiManager.ShowGameOverScreen();
            
        // NOT: Oyuncu objesini Destroy() ile yok ETMİYORUZ. 
        // Yok edersek kamera takibi gibi diğer script'ler hata verebilir.
        // Sadece kontrolünü elinden alıyoruz.
    }
}