using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    //public float damageAmount = 10f;
    private PlayerController playerController; // YENİ
    // public string targetTag = "Enemy"; 

    void Awake()
    {
        // Bu hitbox'ın ait olduğu PlayerController'ı bul
        playerController = GetComponentInParent<PlayerController>();
    }
    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        if (!playerController.HasAlreadyHit(other))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Hasarı PlayerStats'tan al
                int damageToDeal = PlayerStats.instance.totalDamage;
                
                AudioManager.instance.PlayPlayerHitEnemy();
                enemyHealth.TakeDamage(damageToDeal);
                Debug.Log(other.name + " adlı düşmana " + damageToDeal + " hasar verildi!");
                
                playerController.RegisterHit(other);
            }
        }
    }
}
}