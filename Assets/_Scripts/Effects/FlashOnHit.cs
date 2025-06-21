// FlashOnHit.cs
using UnityEngine;
using System.Collections;

public class FlashOnHit : MonoBehaviour
{
    [Header("Flash Settings")]
    [Tooltip("Hasar anında uygulanacak renk.")]
    public Color flashColor = Color.white;
    [Tooltip("Flash efektinin ne kadar süreceği (saniye).")]
    public float flashDuration = 0.1f;

    // --- Private Değişkenler ---
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Material[] originalMaterials;
    private Material flashMaterial;
    private Coroutine flashCoroutine;

    void Awake()
    {
        // Karakterin üzerindeki tüm SkinnedMeshRenderer'ları bul
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderers == null || skinnedMeshRenderers.Length == 0)
        {
            Debug.LogError("No SkinnedMeshRenderer found on this object or its children.");
            enabled = false;
            return;
        }

        // Orijinal materyalleri ve flash materyalini hazırla
        StoreOriginalMaterials();
        CreateFlashMaterial();
    }

    // Karakterin orijinal materyallerini bir dizide sakla
    private void StoreOriginalMaterials()
    {
        originalMaterials = new Material[skinnedMeshRenderers.Length];
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            originalMaterials[i] = skinnedMeshRenderers[i].material;
        }
    }

    // Flash efekti için geçici bir materyal oluştur
    private void CreateFlashMaterial()
    {
        // Orijinal materyallerden birini temel alarak yeni bir materyal oluştur
        if (originalMaterials.Length > 0)
        {
            flashMaterial = new Material(originalMaterials[0]);
            // URP için: Materyalin ana rengini (_BaseColor) değiştir
            if (flashMaterial.HasProperty("_BaseColor"))
            {
                flashMaterial.SetColor("_BaseColor", flashColor);
            }
            // Built-in RP için: Materyalin ana rengini (_Color) değiştir
            else if (flashMaterial.HasProperty("_Color"))
            {
                flashMaterial.SetColor("_Color", flashColor);
            }
        }
    }

    // Dışarıdan çağrılacak olan ana flash fonksiyonu
    public void TriggerFlash()
    {
        // Eğer zaten bir flash efekti çalışıyorsa, onu durdur ve yenisini başlat
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Tüm mesh'lere flash materyalini uygula
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            skinnedMeshRenderers[i].material = flashMaterial;
        }

        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(flashDuration);

        // Tüm mesh'lere orijinal materyallerini geri yükle
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            skinnedMeshRenderers[i].material = originalMaterials[i];
        }

        // Coroutine bitti
        flashCoroutine = null;
    }
}