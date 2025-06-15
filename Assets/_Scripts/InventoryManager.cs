// InventoryManager.cs (GÜNCELLENMİŞ HALİ)
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int inventorySize = 16;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item != null && slot.item == itemToAdd)
                {
                    slot.AddQuantity(1);
                    if(onInventoryChangedCallback != null)
                        onInventoryChangedCallback.Invoke();
                    return true;
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = itemToAdd;
                slot.quantity = 1;
                if(onInventoryChangedCallback != null)
                    onInventoryChangedCallback.Invoke();
                return true;
            }
        }

        Debug.Log("Inventory is full. Could not add " + itemToAdd.itemName);
        return false;
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        InventorySlot slot = slots[slotIndex];
        if (slot.item == null) return;

        if (slot.item.isStackable && slot.quantity > 1)
        {
            slot.RemoveQuantity(1);
        }
        else
        {
            slot.item = null;
            slot.quantity = 0;
        }
        
        if(onInventoryChangedCallback != null)
            onInventoryChangedCallback.Invoke();
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        InventorySlot slot = slots[slotIndex];
        if (slot.item != null)
        {
            slot.item.Use();
            RemoveItem(slotIndex);
        }
    }
}