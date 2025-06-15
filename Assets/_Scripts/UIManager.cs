// UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Yeniden başlatma fonksiyonu için
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHealthSlider;
    // public TMP_Text playerHealthText; // İsteğe bağlı: Canı sayı olarak göstermek için

    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    // public Button restartButton; // Butonun OnClick event'ini Inspector'dan atamak daha kolay

    [Header("Inventory UI")]
    public GameObject inventoryPanel; // Envanter panelinin referansı

    void Start()
    {
        // Oyun başında ilgili panellerin kapalı olduğundan emin ol
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Envanteri açıp kapatmak için input'u burada dinle
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // --- Fonksiyonlar ---

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

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            // Panelin mevcut durumunun tersini yap (açıksa kapat, kapalıysa aç)
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // Bu fonksiyon, "Yeniden Başla" butonunun OnClick event'ine bağlanabilir.
    public void RestartGame()
    {
        // Eğer oyunu durdurduysanız (Time.timeScale = 0), tekrar başlatın
        Time.timeScale = 1f;
        
        // Mevcut sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}