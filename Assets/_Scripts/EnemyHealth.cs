using UnityEngine;
using UnityEngine.UI; // Slider için

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;

    public Slider healthBarSlider;
    public Canvas healthBarCanvasComponent; // Canvas bileşenini doğrudan referans alalım

    // public GameObject deathEffectPrefab;
    // public GameObject itemDropPrefab;

    void Awake() // Start yerine Awake kullanmak, diğer scriptlerin Start'ından önce çalışmasını sağlayabilir.
    {
        if (healthBarCanvasComponent == null)
        {
            // Eğer Inspector'dan atanmadıysa, child objelerden bulmayı dene
            healthBarCanvasComponent = GetComponentInChildren<Canvas>();
        }

        if (healthBarCanvasComponent != null)
        {
            // World Space Canvas'ın Event Camera'sını (aslında worldCamera özelliğidir) ata
            if (healthBarCanvasComponent.renderMode == RenderMode.WorldSpace)
            {
                healthBarCanvasComponent.worldCamera = Camera.main; // Sahnedeki ana kamerayı bul ve ata
            }
        }
        else
        {
            Debug.LogWarning("EnemyHealth: Health Bar Canvas component not found on " + gameObject.name);
        }

        // Canvas'ın içindeki Slider'ı bulmak için daha sağlam bir yol (eğer healthBarSlider Inspector'dan atanmadıysa)
        if (healthBarSlider == null && healthBarCanvasComponent != null)
        {
            healthBarSlider = healthBarCanvasComponent.GetComponentInChildren<Slider>();
            if (healthBarSlider == null)
            {
                 Debug.LogWarning("EnemyHealth: Health Bar Slider not found under the canvas on " + gameObject.name);
            }
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        // Başlangıçta can barını gizle, oyuncu yaklaşınca göster (isteğe bağlı)
        // if (healthBarCanvasComponent != null) healthBarCanvasComponent.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log(gameObject.name + " canı: " + currentHealth);

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        // if (healthBarCanvasComponent != null && !healthBarCanvasComponent.gameObject.activeInHierarchy && currentHealth < maxHealth)
        // {
        //    healthBarCanvasComponent.gameObject.SetActive(true);
        // }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " öldü!");
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (healthBarCanvasComponent != null && healthBarCanvasComponent.gameObject.activeInHierarchy && healthBarCanvasComponent.worldCamera != null)
        {
            // Canvas'ı kameraya doğru baktır
            Transform cameraTransform = healthBarCanvasComponent.worldCamera.transform;
            healthBarCanvasComponent.transform.LookAt(healthBarCanvasComponent.transform.position + cameraTransform.rotation * Vector3.forward,
                                             cameraTransform.rotation * Vector3.up);
        }
    }
}