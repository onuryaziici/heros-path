// PlayerInteractor.cs (SON ve DOĞRU HALİ)
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerInteractor : MonoBehaviour
{
    public List<ItemPickup> interactableItems = new List<ItemPickup>();
    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        // Önce listeyi temizle, sonra en yakını bul
        CleanUpAndFindClosest();

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Eğer hala etkileşime girilecek bir şey varsa (en yakındaki), topla
            if (interactableItems.Count > 0)
            {
                // En yakını tekrar bulmaya gerek yok, CleanUpAndFindClosest zaten buldu.
                // Ama güvenlik için tekrar bulabiliriz.
                var closestItem = GetClosestItem();
                if (closestItem != null)
                {
                    closestItem.PickUp();
                }
            }
        }
    }

    void CleanUpAndFindClosest()
    {
        // 1. Listeden 'null' veya 'missing' olan tüm referansları kaldır.
        interactableItems.RemoveAll(item => item == null);

        // 2. Temizlikten sonra listenin durumuna göre UI'ı güncelle.
        if (interactableItems.Count > 0)
        {
            // Eğer hala eşya varsa, prompt'u göster.
            uiManager?.ShowInteractionPrompt(true);
            
            // İsteğe bağlı: En yakın eşyaya bir vurgu/outline efekti burada tetiklenebilir.
            // ItemPickup closest = GetClosestItem();
        }
        else
        {
            // Eğer hiç eşya kalmadıysa, prompt'u gizle.
            uiManager?.ShowInteractionPrompt(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null && !interactableItems.Contains(item))
        {
            interactableItems.Add(item);
        }
    }

    void OnTriggerExit(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            interactableItems.Remove(item);
        }
    }

    private ItemPickup GetClosestItem()
    {
        // Bu fonksiyon artık sadece sıralama ve döndürme işini yapıyor.
        // Temizlik ve UI güncellemesi Update'teki CleanUpAndFindClosest'e taşındı.
        if (interactableItems.Count == 0) return null;
        
        return interactableItems.OrderBy(item => Vector3.Distance(transform.position, item.transform.position)).FirstOrDefault();
    }
}