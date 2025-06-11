// InventoryManager.cs
using System.Collections.Generic; // List kullanmak için
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Singleton pattern: Bu script'e her yerden kolayca erişim sağlamak için.
    public static InventoryManager instance;
    public Item testItemToAdd; // Inspector'dan bir "Küçük Sağlık İksiri" asset'ini buraya sürükleyin
    public int inventorySize = 16; // Envanterde kaç slot olacağı
    public List<InventorySlot> slots = new List<InventorySlot>();

    // Event/Delegate: Envanter güncellendiğinde UI'ın haberdar olması için.
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;
void Update()
{
    // "I" tuşuna basıldığında test eşyasını envantere ekle
    if (Input.GetKeyDown(KeyCode.I))
    {
        if (testItemToAdd != null)
        {
            AddItem(testItemToAdd);
        }
    }

    // "U" tuşuna basıldığında 0. slottaki eşyayı kullan
    if (Input.GetKeyDown(KeyCode.U))
    {
        UseItem(0);
    }
}

    void Awake()
    {
        // Singleton kurulumu
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Envanter slotlarını başlangıçta boş olarak oluştur
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }

    // Envantere eşya ekleyen ana fonksiyon
    public bool AddItem(Item itemToAdd)
    {
        // --- 1. Adım: Eğer eşya stacklenebilirse, mevcut bir slota eklemeyi dene ---
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                // Eğer slot boş değilse, içindeki eşya eklemek istediğimiz eşya ile aynıysa
                // ve o slota daha fazla eşya sığıyorsa (bu projede limit koymuyoruz ama ileride eklenebilir)
                if (slot.item != null && slot.item == itemToAdd)
                {
                    slot.AddQuantity(1);
                    Debug.Log(itemToAdd.itemName + " added to an existing stack.");
                    
                    // Envanterin değiştiğini UI'a bildir
                    if(onInventoryChangedCallback != null)
                        onInventoryChangedCallback.Invoke();
                        
                    return true; // Ekleme başarılı
                }
            }
        }

        // --- 2. Adım: Eğer stacklenemiyorsa veya mevcut slot bulunamadıysa, boş bir slota ekle ---
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) // Eğer slot boşsa
            {
                slot.item = itemToAdd;
                slot.quantity = 1;
                Debug.Log(itemToAdd.itemName + " added to a new slot.");

                // Envanterin değiştiğini UI'a bildir
                if(onInventoryChangedCallback != null)
                    onInventoryChangedCallback.Invoke();

                return true; // Ekleme başarılı
            }
        }

        // --- 3. Adım: Eğer envanterde hiç yer yoksa ---
        Debug.Log("Inventory is full. Could not add " + itemToAdd.itemName);
        return false; // Ekleme başarısız
    }

    // Belirtilen slottan eşyayı kaldıran fonksiyon
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

        Debug.Log("Item removed from slot " + slotIndex);
        
        // Envanterin değiştiğini UI'a bildir
        if(onInventoryChangedCallback != null)
            onInventoryChangedCallback.Invoke();
    }

    // Belirtilen slottaki eşyayı kullanan fonksiyon
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;

        InventorySlot slot = slots[slotIndex];
        if (slot.item != null)
        {
            // Item'in (veya HealthPotion'ın) kendi Use() fonksiyonunu çağır
            slot.item.Use();

            // Kullandıktan sonra eşyayı envanterden kaldır/azalt
            RemoveItem(slotIndex);
        }
    }
}