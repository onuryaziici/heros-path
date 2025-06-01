using UnityEngine;
using UnityEngine.UI;
using TMPro; // Eğer TextMeshPro kullanılırsa
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHealthSlider;
    // public TMP_Text playerHealthText; // İsteğe bağlı: Canı sayı olarak göstermek için

    // [Header("Enemy UI")] - Düşman can barı için daha sonra
    // public Slider enemyHealthSlider;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    // public Button restartButton; // Daha sonra eklenecek

    void Start()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false); // Oyun başında Oyun Bitti ekranını kapat
    }

    public void UpdatePlayerHealth(float currentHealth, float maxHealth)
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = maxHealth;
            playerHealthSlider.value = currentHealth;
        }

        // if (playerHealthText != null)
        // {
        //     playerHealthText.text = Mathf.RoundToInt(currentHealth) + " / " + Mathf.RoundToInt(maxHealth);
        // }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
    }

    public void RestartGame() // Butona bağlanacak fonksiyon (daha sonra)
    {
        Time.timeScale = 1f; // Oyunu tekrar akıcı hale getir
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}