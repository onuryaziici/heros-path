using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth;

    // public GameObject deathEffectPrefab; // Ölüm efekti için (isteğe bağlı)
    // public GameObject itemDropPrefab; // Eşya düşürme için (isteğe bağlı)

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " canı: " + currentHealth);

        // animator.SetTrigger("Hurt"); // Hasar alma animasyonu (daha sonra)

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " öldü!");
        // if (deathEffectPrefab != null)
        //     Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // if (itemDropPrefab != null) // GDD: "rastgele bir eşya düşürme (isteğe bağlı ama iyi olur)"
        // {
        //     Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        // }

        Destroy(gameObject); // Düşmanı yok et
    }
}