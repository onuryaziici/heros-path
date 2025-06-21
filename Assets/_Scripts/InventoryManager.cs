// InventoryManager.cs
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int inventorySize = 16;
    public List<InventorySlot> slots = new List<InventorySlot>();

    // UI'ın envanterdeki değişiklikleri dinlemesi için bir event (olay)
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Awake()
    {
        // Singleton Pattern Kurulumu
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Envanteri başlangıçta boş slotlarla doldur
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        // Stacklenebilir eşyalar için mevcut bir yığın ara
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item != null && slot.item == itemToAdd)
                {
                    slot.AddQuantity(1);
                    onInventoryChangedCallback?.Invoke(); // UI'ı güncelle
                    return true;
                }
            }
        }

        // Stacklenemeyen eşyalar veya yeni yığınlar için boş bir slot ara
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = itemToAdd;
                slot.quantity = 1;
                onInventoryChangedCallback?.Invoke(); // UI'ı güncelle
                return true;
            }
        }

        Debug.Log("Inventory is full. Could not add " + itemToAdd.itemName);
        return false;
    }

    // Belirtilen slottan eşyayı kaldıran fonksiyon (indekse göre)
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        
        InventorySlot slot = slots[slotIndex];
        if (slot.item == null) return;

        // Eğer eşya stackleniyorsa ve birden fazla varsa, sadece sayısını azalt
        if (slot.item.isStackable && slot.quantity > 1)
        {
            slot.RemoveQuantity(1);
        }
        // Aksi takdirde (stacklenemiyorsa veya sadece 1 tane varsa), slotu tamamen boşalt
        else
        {
            slot.item = null;
            slot.quantity = 0;
        }
        
        onInventoryChangedCallback?.Invoke(); // UI'ı güncelle
    }

    // YENİ FONKSİYON: Belirli bir Item referansını envanterden bulup kaldırır.
    // Bu, EquipmentManager tarafından kullanılacak.
    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == itemToRemove)
            {
                // Eşyanın bulunduğu indeksi bulduk, şimdi o indekse göre
                // kaldıran diğer RemoveItem fonksiyonumuzu çağırabiliriz.
                RemoveItem(i);
                return; // Eşyayı bulup kaldırdık, fonksiyondan çık.
            }
        }
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;

        InventorySlot slot = slots[slotIndex];
        if (slot.item != null)
        {
            // Eşyanın kendi Use() fonksiyonunu çağır (bu, iksir için Heal, silah için Equip olacak).
            slot.item.Use();
            
            // Not: Kuşanılan silahın envanterden silinmesi işlemini artık EquipmentManager
            // kendi içindeki Equip fonksiyonunda, RemoveItem(item) çağrısıyla yapıyor.
            // Bu yüzden UseItem fonksiyonunda tekrar bir silme işlemi yapmamıza gerek kalmadı,
            // çünkü silahın Use() metodu zaten EquipmentManager'ı tetikliyor.
            // İksir gibi tüketilen eşyalar için ise, iksirin Use() metodu bir şey yapmazken,
            // buradaki RemoveItem(slotIndex) çağrısı onu envanterden siler.
            
            // Ancak, silahlar kuşanıldığında envanterden silineceği için,
            // bu yapı iksir gibi "tüketilen" eşyalar için de çalışır.
            // UseItem, item.Use() dedikten sonra, eğer o item bir silâhsa, EquipmentManager 
            // onu envanterden kaldırır. Eğer bir iksirse, EquipmentManager bir şey yapmaz,
            // ama yine de bu fonksiyonun sonunda onu envanterden kaldırmamız gerekir.
            // Weapon.Use() zaten RemoveItem'ı tetiklediği için, burada bir kontrol ekleyelim.
            if (!(slot.item is Weapon)) // Eğer kullanılan eşya bir Weapon DEĞİLSE, buradan kaldır.
            {
                RemoveItem(slotIndex);
            }
        }
    }
}