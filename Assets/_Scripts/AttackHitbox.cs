using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float damageAmount = 10f;
    // public string targetTag = "Enemy"; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Debug.Log(other.name + " adlı düşmana " + damageAmount + " hasar verildi!");
                // Bu hitbox bir düşmana çarptıktan sonra hemen deaktif edilebilir ki tek saldırıda birden fazla vurmasın
                // gameObject.SetActive(false); // Eğer tek vuruşta deaktif olmasını isterseniz.
                                            // Ama PlayerController'daki coroutine zaten kısa bir süre sonra kapatıyor.
            }
        }
    }
}