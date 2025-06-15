// EnemyHealth.cs
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic; // List kullanmak için gerekli

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 30f;
    [SerializeField]
    private float currentHealth;

    [Header("UI")]
    public Slider healthBarSlider;
    public Canvas healthBarCanvasComponent;

    [Header("Death")]
    public float timeBeforeDestroy = 2.5f;

    [Header("Loot Settings")] // YENİ EKLENEN BÖLÜM
    [Tooltip("Öldüğünde düşürebileceği eşyaların prefab listesi.")]
    public List<GameObject> lootTable; 
    [Tooltip("Listeden bir eşya düşürme ihtimali (0=asla, 1=her zaman).")]
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    // --- Private Değişkenler ---
    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning(gameObject.name + " üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        }

        if (healthBarCanvasComponent == null)
        {
            healthBarCanvasComponent = GetComponentInChildren<Canvas>();
        }

        if (healthBarCanvasComponent != null)
        {
            if (healthBarCanvasComponent.renderMode == RenderMode.WorldSpace)
            {
                healthBarCanvasComponent.worldCamera = Camera.main;
            }
        }
        else
        {
            Debug.LogWarning("EnemyHealth: Health Bar Canvas component not found on " + gameObject.name);
        }
        
        if (healthBarSlider == null && healthBarCanvasComponent != null)
        {
            healthBarSlider = healthBarCanvasComponent.GetComponentInChildren<Slider>();
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
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (animator != null && currentHealth > 0)
        {
            // Eğer Hurt animasyonunuz yoksa ve hata alıyorsanız bu satırı yorum satırı yapın veya silin.
            // animator.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(gameObject.name + " öldü!");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (healthBarCanvasComponent != null)
        {
            healthBarCanvasComponent.gameObject.SetActive(false);
        }

        Collider mainCollider = GetComponent<Collider>();
        if (mainCollider != null) mainCollider.enabled = false;
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        
        EnemyAI aiScript = GetComponent<EnemyAI>();
        if(aiScript != null) aiScript.enabled = false;

        // Ölüm anında eşya düşürme fonksiyonunu çağır
        DropLoot(); // YENİ EKLENEN FONKSİYON ÇAĞRISI

        Destroy(gameObject, timeBeforeDestroy);
    }

    // YENİ EKLENEN FONKSİYON
    void DropLoot()
    {
        // Eğer düşürecek eşya listesi boşsa veya şans tutmazsa, fonksiyondan çık.
        if (lootTable == null || lootTable.Count == 0 || Random.value > dropChance)
        {
            return;
        }

        // Loot tablosundan rastgele bir eşya seç
        int randomIndex = Random.Range(0, lootTable.Count);
        GameObject itemToDropPrefab = lootTable[randomIndex];

        if (itemToDropPrefab != null)
        {
            // Seçilen eşya prefab'ını, düşmanın öldüğü yerde oluştur (instantiate et).
            // Y ekseninde biraz yukarıda oluşturmak, zeminin içine girmesini engelleyebilir.
            Vector3 dropPosition = transform.position + Vector3.up * 0.5f;
            Instantiate(itemToDropPrefab, dropPosition, Quaternion.identity);
            Debug.Log(itemToDropPrefab.name + " dropped!");
        }
    }
    
    void LateUpdate()
    {
        if (isDead) return;

        if (healthBarCanvasComponent != null && healthBarCanvasComponent.gameObject.activeInHierarchy && healthBarCanvasComponent.worldCamera != null)
        {
            Transform cameraTransform = healthBarCanvasComponent.worldCamera.transform;
            healthBarCanvasComponent.transform.LookAt(healthBarCanvasComponent.transform.position + cameraTransform.rotation * Vector3.forward,
                                                     cameraTransform.rotation * Vector3.up);
        }
    }
}