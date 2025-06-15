// InventoryUI.cs
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform slotsParent; // Slotların oluşturulacağı parent obje (Grid Layout Group olan)
    public GameObject slotPrefab; // Slot prefab'ı

    // --- Private Değişkenler ---
    private InventoryManager inventoryManager;
    private InventorySlotUI[] slotUIs; // Sahnedeki tüm UI slotlarını tutan dizi

    void Start()
    {
        // Gerekli referansları al
        inventoryManager = InventoryManager.instance;
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager instance not found!");
            return;
        }

        // Envanter verileri değiştiğinde bizim UpdateUI metodumuzu çağıracak şekilde event'e abone ol.
        inventoryManager.onInventoryChangedCallback += UpdateUI;

        // Envanter boyutu kadar UI slotu oluştur
        InitializeSlots();

        // Başlangıçta UI'ı bir kez güncelle (eğer oyuna eşyayla başlanırsa diye)
        UpdateUI();
    }

    // Başlangıçta slot prefab'larını instantiate eden fonksiyon
    void InitializeSlots()
    {
        slotUIs = new InventorySlotUI[inventoryManager.inventorySize];
        for (int i = 0; i < inventoryManager.inventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            slotUIs[i] = slotGO.GetComponent<InventorySlotUI>();
            if (slotUIs[i] != null)
            {
                // Her slota kendi indeksini bildir, böylece hangi slota tıklandığını bilir.
                slotUIs[i].Setup(i);
            }
        }
    }

    // Envanter verileri değiştiğinde UI'ı güncelleyen ana fonksiyon
    void UpdateUI()
    {
        // Tüm UI slotlarını döngüye al
        for (int i = 0; i < slotUIs.Length; i++)
        {
            // Eğer ilgili mantıksal slotta bir eşya varsa...
            if (i < inventoryManager.slots.Count && inventoryManager.slots[i].item != null)
            {
                // UI slotuna eşya bilgilerini gönder
                slotUIs[i].AddItem(inventoryManager.slots[i]);
            }
            else
            {
                // Eğer mantıksal slot boşsa, UI slotunu temizle
                slotUIs[i].ClearSlot();
            }
        }
    }

    // Bu script yok edildiğinde (örn: sahne değiştiğinde) event aboneliğini iptal et.
    // Bu, memory leak (hafıza sızıntısı) ve hataları önler.
    void OnDestroy()
    {
        if (inventoryManager != null)
        {
            inventoryManager.onInventoryChangedCallback -= UpdateUI;
        }
    }
}