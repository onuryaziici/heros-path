// UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHealthSlider;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;

    [Header("Inventory UI")]
    public GameObject inventoryPanel;

    [Header("Interaction UI")]
    [Tooltip("Toplanabilir bir eşyanın yanındayken görünecek UI elemanı.")]
    public GameObject interactionPrompt; // Etkileşim yazısının referansı

    void Start()
    {
        // Oyun başında ilgili panellerin kapalı olduğundan emin ol
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
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
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // Bu fonksiyon, etkileşim yazısını göstermek veya gizlemek için dışarıdan (PlayerInteractor'dan) çağrılacak.
    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null && interactionPrompt.activeSelf != show)
        {
            interactionPrompt.SetActive(show);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}