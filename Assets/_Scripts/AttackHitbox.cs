using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float damageAmount = 10f;
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
            // PlayerController'a bu düşmana daha önce vurulup vurulmadığını sor.
            // Eğer daha önce vurulmadıysa...
            if (!playerController.HasAlreadyHit(other))
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log(other.name + " adlı düşmana " + damageAmount + " hasar verildi!");
                    
                    // Bu düşmanı "vuruldu" olarak kaydet.
                    playerController.RegisterHit(other);
                }
            }
        }
    }
}