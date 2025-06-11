// HealthPotion.cs
using UnityEngine;

// Create menüsüne Sağlık İksiri için de ayrı bir seçenek ekleyelim.
[CreateAssetMenu(fileName = "New Health Potion", menuName = "Inventory/Health Potion")]
public class HealthPotion : Item
{
    public int healAmount = 25; // Ne kadar can yenileyeceği

    // "Item" sınıfındaki sanal Use fonksiyonunu burada override ediyoruz (ezip üzerine yazıyoruz).
    // Bu, sağlık iksirine tıklandığında ne olacağını belirler.
    public override void Use()
    {
        base.Use(); // Bu, Item.cs'deki Use() fonksiyonunu çağırır (Debug.Log mesajı için).

        // Oyuncuyu bul ve PlayerHealth script'ine eriş.
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        // Eğer oyuncu bulunduysa ve üzerinde PlayerHealth script'i varsa, canını yenile.
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log(itemName + " used. Healed for " + healAmount + " HP.");

            // Not: Eşyanın envanterden silinmesi işlemini burada DEĞİL,
            // envanteri yöneten InventoryManager script'inde yapacağız.
        }
        else
        {
            Debug.LogWarning("Player not found or does not have a PlayerHealth component.");
        }
    }
}