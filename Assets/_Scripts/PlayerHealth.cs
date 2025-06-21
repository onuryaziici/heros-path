// PlayerHealth.cs
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField]
    private float currentHealth;

    [Header("Dependencies")]
    public UIManager uiManager;

    // --- Private Değişkenler ---
    private Animator animator;
    private PlayerController playerController; // YENİ: PlayerController referansı
    private bool isDead = false;
    private FlashOnHit flashEffect; // Flash script'ine referans

    void Start()
    {
        flashEffect = GetComponent<FlashOnHit>(); // Başlangıçta referansı al
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>(); // YENİ: Başlangıçta referansı al

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        if (animator == null)
            Debug.LogWarning("Player üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        if (playerController == null)
            Debug.LogError("Player üzerinde PlayerController bileşeni bulunamadı!");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        AudioManager.instance.PlayPlayerTakeDamage(); // Hasar alma sesi
        if (flashEffect != null)
        {
            flashEffect.TriggerFlash();
        }

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        // Eğer hala hayattaysa, hasar alma animasyonunu ve sersemleme durumunu tetikle
        if (currentHealth > 0)
        {
            // PlayerController'a hasar aldığını bildirerek kontrolü kısa süreliğine elinden al
            if (playerController != null)
            {
                playerController.TriggerHurtState();
            }
        }
        else // Eğer canı bittiyse, öl
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Oyuncu Öldü! Oyun Bitti.");
        // YENİ: Ölüm sesini çal
        AudioManager.instance.PlayPlayerDeath(); 

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (playerController != null)
            playerController.enabled = false;
        
        CharacterController charController = GetComponent<CharacterController>();
        if(charController != null)
            charController.enabled = false;

        if (uiManager != null)
            uiManager.ShowGameOverScreen();
    }
}