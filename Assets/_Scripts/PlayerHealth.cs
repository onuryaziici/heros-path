using UnityEngine;
// using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UIManager uiManager; // UIManager'a referans

    // public GameObject gameOverScreen; // Bu satırı kaldırabiliriz, UIManager yönetecek

    void Start()
    {
        currentHealth = maxHealth;
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>(); // Sahnedeki UIManager'ı bul
            if (uiManager == null)
                Debug.LogError("Sahnede UIManager bulunamadı!");
        }

        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0; // Can eksiye düşmesin

        Debug.Log(gameObject.name + " canı: " + currentHealth);
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Oyuncu iyileşti: " + currentHealth);
        if (uiManager != null)
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("Oyuncu Öldü! Oyun Bitti.");
        if (uiManager != null)
            uiManager.ShowGameOverScreen();

        // Oyuncu kontrolünü devre dışı bırak vs.
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = false;

        // Time.timeScale = 0f; // Oyunu durdurmak için (daha sonra düşünülebilir)
    }
}