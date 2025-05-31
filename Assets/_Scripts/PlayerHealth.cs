using UnityEngine;
// using UnityEngine.SceneManagement; // Oyun bitti ekranı için (daha sonra)

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // public UIManager uiManager; // Can barını güncellemek için (daha sonra)
    // public GameObject gameOverScreen; // (daha sonra)

    void Start()
    {
        currentHealth = maxHealth;
        // if (uiManager != null)
        //     uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
        // if (gameOverScreen != null)
        //     gameOverScreen.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Oyuncu canı: " + currentHealth);
        // uiManager.UpdatePlayerHealth(currentHealth, maxHealth); // UI Güncellemesi
        // animator.SetTrigger("Hurt"); // Oyuncu hasar alma animasyonu

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount) // İksir için
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Oyuncu iyileşti: " + currentHealth);
        // uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
    }


    void Die()
    {
        Debug.Log("Oyuncu Öldü! Oyun Bitti.");
        // gameOverScreen.SetActive(true);
        // Time.timeScale = 0f; // Oyunu durdur
        // Veya
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Sahneyi yeniden başlat
        // Şimdilik sadece konsola yazsın
        GetComponent<PlayerController>().enabled = false; // Oyuncu kontrolünü devre dışı bırak
    }
}