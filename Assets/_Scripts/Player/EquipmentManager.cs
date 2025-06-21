// EquipmentManager.cs
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Weapon currentWeapon;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Bir silahı kuşanan ana fonksiyon
    public void Equip(Weapon newWeapon)
    {
        // Eğer zaten bir silah varsa, onu envantere geri koy (şimdilik basitçe logluyoruz)
        if (currentWeapon != null)
        {
            Debug.Log("Unequipped " + currentWeapon.itemName);
            // İleri seviye: InventoryManager.instance.AddItem(currentWeapon);
        }

        currentWeapon = newWeapon;
        Debug.Log(newWeapon.itemName + " is now equipped.");

        // Statları yeniden hesaplaması için PlayerStats'a haber ver
        PlayerStats.instance.CalculateStats();

        // Envanterden kuşanılan silahı çıkar
        // Not: Bu basit bir yapı. Normalde ekipman slotları envanterden ayrı olur.
        // Prototipimiz için, kuşanılan silah envanterden kaybolacak.
        InventoryManager.instance.RemoveItem(newWeapon); 
    }
}