// CameraController.cs
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Takip edilecek obje (Player)
    public Vector3 offset = new Vector3(0f, 10f, -7f); // Kameranın oyuncuya göre konumu
    public float smoothSpeed = 0.125f; // Kamera takip yumuşaklığı

    void LateUpdate() // Karakter hareket ettikten sonra çalışması için LateUpdate
    {
        if (target == null)
        {
            Debug.LogWarning("Kamera için hedef (target) atanmamış!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // İsteğe bağlı: Kamera her zaman oyuncuya baksın
        // transform.LookAt(target);
        // Ancak sabit bir top-down için yukarıdaki offset ve kameranın kendi rotasyonu yeterli olabilir.
        // Eğer LookAt kullanacaksanız, kameranın ilk rotasyonunu script ile override edebilir.
        // Bu durumda kameranın ilk rotasyonunu (örneğin X:60) offset ile birlikte düşünmeniz gerekebilir.
        // Şimdilik sabit rotasyon ve offset ile devam edelim.
    }
}