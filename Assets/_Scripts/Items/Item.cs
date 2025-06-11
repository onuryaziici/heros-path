using UnityEngine;

// Bu satır, Unity'nin "Assets/Create" menüsüne yeni bir seçenek ekler.
// Artık proje penceresinde sağ tıklayıp "Create > Inventory > Item" diyerek yeni eşya asset'leri oluşturabileceğiz.
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // Tüm eşyaların sahip olacağı ortak özellikler
    public string itemName = "New Item";
    [TextArea(3,10)] // Inspector'da daha geniş bir metin alanı sağlar
    public string itemDescription = "Item Description";
    public Sprite itemIcon = null;

    // Eşyanın envanterde stacklenip (üst üste birikip) birikemeyeceği
    public bool isStackable = true;

    // Sanal fonksiyonlar, bu sınıftan türeyecek diğer eşya tiplerinin (iksir, silah vb.)
    // kendilerine özgü "kullanma" davranışlarını tanımlamasını sağlar.
    public virtual void Use()
    {
        // Bu temel "Item" sınıfı için "Use" fonksiyonu bir şey yapmaz.
        // Sadece konsola bir mesaj yazdırabiliriz.
        Debug.Log("Using " + itemName);
    }
}