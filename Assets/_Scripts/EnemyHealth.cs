// EnemyHealth.cs
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [Header("Loot Settings")]
    public List<GameObject> lootTable; 
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    // --- Private Değişkenler ---
    private Animator animator;
    private EnemyAI enemyAI; // YENİ: EnemyAI referansı
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAI = GetComponent<EnemyAI>(); // YENİ: Başlangıçta referansı al

        if (animator == null)
            Debug.LogWarning(gameObject.name + " üzerinde veya child objelerinde Animator bileşeni bulunamadı!");
        if (enemyAI == null)
            Debug.LogError(gameObject.name + " üzerinde EnemyAI script'i bulunamadı!");

        // ... Diğer Awake kodları ...
        if (healthBarCanvasComponent == null) healthBarCanvasComponent = GetComponentInChildren<Canvas>();
        if (healthBarCanvasComponent != null)
        {
            if (healthBarCanvasComponent.renderMode == RenderMode.WorldSpace)
                healthBarCanvasComponent.worldCamera = Camera.main;
        }
        if (healthBarSlider == null && healthBarCanvasComponent != null)
            healthBarSlider = healthBarCanvasComponent.GetComponentInChildren<Slider>();
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
        if (currentHealth < 0) currentHealth = 0;

        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (currentHealth > 0)
        {
            if (animator != null)
                animator.SetTrigger("Hurt");
            
            // Düşmanın sersemlemesi için EnemyAI'a haber ver
            if (enemyAI != null)
            {
                enemyAI.TriggerHurtState();
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        
        if (animator != null)
            animator.SetTrigger("Die");
        
        if (healthBarCanvasComponent != null)
            healthBarCanvasComponent.gameObject.SetActive(false);
        
        Collider mainCollider = GetComponent<Collider>();
        if (mainCollider != null) mainCollider.enabled = false;
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        
        if(enemyAI != null) enemyAI.enabled = false;
        AudioManager.instance.PlayEnemyDie(transform.position); // Düşmanın pozisyonunda ölüm sesi
        DropLoot();
        Destroy(gameObject, timeBeforeDestroy);
    }

    void DropLoot()
    {
        if (lootTable == null || lootTable.Count == 0 || Random.value > dropChance) return;
        int randomIndex = Random.Range(0, lootTable.Count);
        GameObject itemToDropPrefab = lootTable[randomIndex];
        if (itemToDropPrefab != null)
        {
            Vector3 dropPosition = transform.position + Vector3.up * 0.5f;
            Instantiate(itemToDropPrefab, dropPosition, Quaternion.identity);
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