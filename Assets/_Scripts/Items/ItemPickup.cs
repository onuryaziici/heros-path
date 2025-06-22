// ItemPickup.cs (BASİTLEŞTİRİLMİŞ HALİ)
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    public Item item;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void PickUp()
    {
        bool wasPickedUp = InventoryManager.instance.AddItem(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    }
}