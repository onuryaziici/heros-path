// PlayerInteractor.cs
using UnityEngine;
using System.Collections.Generic; // List kullanmak için
using System.Linq; // OrderBy (sıralama) ve FirstOrDefault gibi LINQ fonksiyonları için

public class PlayerInteractor : MonoBehaviour
{
    // Etkileşim menzilindeki toplanabilir eşyaların listesi
    private List<ItemPickup> interactableItems = new List<ItemPickup>();
    private UIManager uiManager;

    void Start()
    {
        // UIManager'ı başlangıçta bul ve referansını al. Bu, FindObjectOfType'a göre daha performanslıdır.
        // Eğer sahnede UIManager objesine "UIManager" tag'i verirseniz FindGameObjectWithTag da kullanabilirsiniz.
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene! Interaction prompts will not work.");
        }
    }

    void Update()
    {
        // Eğer "E" tuşuna basıldıysa...
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Eğer etkileşim listesinde potansiyel olarak en az bir eşya varsa...
            if (interactableItems.Count > 0)
            {
                // En yakındaki geçerli eşyayı bulmayı dene
                ItemPickup closestItem = GetClosestItem();
                
                // Eğer geçerli bir en yakın eşya bulunduysa, topla
                if (closestItem != null)
                {
                    closestItem.PickUp();
                }
            }
        }
    }

    // Oyuncunun etkileşim trigger'ına bir obje girdiğinde
    void OnTriggerEnter(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            // Eşyanın listede olmadığından emin ol ve ekle
            if (!interactableItems.Contains(item))
            {
                interactableItems.Add(item);

                // Eğer bu, etkileşim alanına giren İLK geçerli eşyaysa, UI yazısını göster
                if (interactableItems.Count == 1 && uiManager != null)
                {
                    uiManager.ShowInteractionPrompt(true);
                }
            }
        }
    }

    // Oyuncunun etkileşim trigger'ından bir obje çıktığında
    void OnTriggerExit(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null)
        {
            // Eşyayı listeden çıkar
            interactableItems.Remove(item);
            
            // Eğer etkileşim alanında HİÇ geçerli eşya kalmadıysa, UI yazısını gizle
            if (interactableItems.Count == 0 && uiManager != null)
            {
                uiManager.ShowInteractionPrompt(false);
            }
        }
    }

    // Listede oyuncuya en yakın olan eşyayı bulan ve UI'ı güncelleyen fonksiyon
    private ItemPickup GetClosestItem()
    {
        // 1. ADIM: Listeden, oyuncu tarafından toplandığı için veya başka bir nedenle 
        // yok olduğu için 'null' hale gelmiş tüm referansları temizle.
        interactableItems.RemoveAll(item => item == null);

        // 2. ADIM: Temizlikten sonra listede hala etkileşime girilebilecek bir eşya kalıp kalmadığını kontrol et.
        if (interactableItems.Count == 0)
        {
            // Eğer listede hiç eşya kalmadıysa, "E'ye bas" yazısını KESİNLİKLE kapat.
            if (uiManager != null)
            {
                uiManager.ShowInteractionPrompt(false);
            }
            // Ve fonksiyondan çık, çünkü en yakın eşya diye bir şey yok.
            return null;
        }

        // 3. ADIM: Eğer listede hala eşyalar varsa, en yakın olanı bul ve döndür.
        // Eğer sadece bir eleman varsa, sıralamaya gerek yok, direkt onu döndür.
        if (interactableItems.Count == 1)
        {
            return interactableItems[0];
        }
        
        // Birden fazla eleman varsa, listeyi oyuncuya olan mesafeye göre sırala ve en yakın olanı (ilkini) döndür.
        return interactableItems.OrderBy(item => Vector3.Distance(transform.position, item.transform.position)).FirstOrDefault();
    }
}