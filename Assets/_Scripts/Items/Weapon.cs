// Weapon.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    [Header("Weapon Stats")]
    public int damageBonus = 5; // Bu silah kuşanıldığında ne kadar ek hasar vereceği

    // Silah "kullanılmaz", "kuşanılır". Bu yüzden Use() fonksiyonu
    // EquipmentManager'a bu silahı kuşanmasını söyleyecek.
    public override void Use()
    {
        // base.Use(); // base.Use() "Using [ItemName]" yazar, bunu istemeyebiliriz.
        Debug.Log("Equipping " + itemName);
        
        // EquipmentManager'a bu silahı kuşanması için haber ver
        EquipmentManager.instance.Equip(this);
    }
}