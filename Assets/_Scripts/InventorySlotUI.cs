// InventorySlotUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;

    private int slotIndex;

    // Bu slotun hangi indekse ait olduğunu belirler
    public void Setup(int index)
    {
        slotIndex = index;
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    // UI'a eşya bilgilerini ekler
    public void AddItem(InventorySlot slot)
    {
        itemIcon.sprite = slot.item.itemIcon;
        itemIcon.enabled = true;

        if (slot.item.isStackable && slot.quantity > 1)
        {
            quantityText.text = slot.quantity.ToString();
            quantityText.enabled = true;
        }
        else
        {
            quantityText.enabled = false;
        }
    }

    // UI slotunu temizler
    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        quantityText.enabled = false;
    }

    // Slota tıklandığında ne olacağı
    private void OnSlotClicked()
    {
        // InventoryManager'daki ilgili eşyayı kullan
        InventoryManager.instance.UseItem(slotIndex);
        Debug.Log("Clicked on slot " + slotIndex);
    }
}