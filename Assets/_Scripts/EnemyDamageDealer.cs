// EnemyDamageDealer.cs
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    // Bu script, EnemyAI'dan hasar miktarını alacak
    private float damage;
    private EnemyAI parentAI;

    void Start()
    {
        // Kendisini oluşturan EnemyAI script'ini bul ve hasar miktarını al
        parentAI = GetComponentInParent<EnemyAI>();
        if (parentAI != null)
        {
            damage = parentAI.damageAmount;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Eğer hitbox "Player" etiketli bir objeye çarparsa...
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy hitbox dealt " + damage + " damage to player.");
                
                // Tek bir saldırıda birden fazla hasar vermemek için hitbox'ı hemen kapat
                gameObject.SetActive(false); 
                // Veya collider'ı kapat
                // GetComponent<Collider>().enabled = false;
            }
        }
    }
}