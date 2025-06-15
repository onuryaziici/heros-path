// TestItemGiver.cs
using UnityEngine;
using System.Collections.Generic; // List kullanmak için

public class TestItemGiver : MonoBehaviour
{
    public List<Item> itemsToGive; // Inspector'dan test etmek istediğiniz eşyaları (iksir vb.) buraya sürükleyin
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = InventoryManager.instance;
    }

    void Update()
    {
        // 1 tuşuna basıldığında listedeki ilk eşyayı ver
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GiveItem(0);
        }

        // 2 tuşuna basıldığında listedeki ikinci eşyayı ver
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GiveItem(1);
        }
        
        // ... istediğiniz kadar tuş ekleyebilirsiniz ...
    }

    void GiveItem(int itemIndex)
    {
        if (inventoryManager != null && itemsToGive != null && itemsToGive.Count > itemIndex)
        {
            Item item = itemsToGive[itemIndex];
            if (item != null)
            {
                bool wasAdded = inventoryManager.AddItem(item);
                if (wasAdded)
                {
                    Debug.Log("Gave 1x " + item.itemName + " to player.");
                }
            }
        }
    }
}