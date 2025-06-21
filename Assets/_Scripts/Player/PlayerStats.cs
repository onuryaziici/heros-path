// PlayerStats.cs
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Bu script'e her yerden kolayca erişim için Singleton
    public static PlayerStats instance;

    [Header("Base Stats")]
    public int baseDamage = 10; // Oyuncunun silahsız temel hasarı

    // Toplam statlar
    public int totalDamage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Başlangıçta toplam hasarı temel hasara eşitle
        CalculateStats();
    }

    public void CalculateStats()
    {
        // Şimdilik sadece hasar var, ama ileride zırh vb. eklenebilir.
        totalDamage = baseDamage;

        // Eğer bir silah kuşanılıyorsa, onun bonusunu ekle
        Weapon equippedWeapon = EquipmentManager.instance?.currentWeapon;
        if (equippedWeapon != null)
        {
            totalDamage += equippedWeapon.damageBonus;
        }

        Debug.Log("Player total damage is now: " + totalDamage);
    }
}