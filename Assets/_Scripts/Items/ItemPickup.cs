// ItemPickup.cs
using UnityEngine;

[RequireComponent(typeof(Collider))] 
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    [Tooltip("Bu objenin temsil ettiği ScriptableObject tabanlı eşya.")]
    public Item item;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void PickUp()
    {
        Debug.Log("Attempting to pick up " + item.itemName);
        
        bool wasPickedUp = InventoryManager.instance.AddItem(item);

        if (wasPickedUp)
        {
            // Eşya başarıyla alındıysa, bu objeyi sahneden yok et.
            // PlayerInteractor, bir sonraki GetClosestItem çağrısında bu null objeyi listeden temizleyecektir.
            AudioManager.instance.PlayItemPickup(); // Eşya toplama sesi
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full. Cannot pick up " + item.itemName);
        }
    }

    void OnDrawGizmos()
    {
        if (item != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}