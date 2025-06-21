// CameraController.cs
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Takip edilecek obje (Player)

    [Header("Orbit Settings")]
    public float distance = 5.0f; // Kameranın hedefe olan varsayılan mesafesi
    public float xSpeed = 120.0f; // Farenin yatay hızı
    public float ySpeed = 120.0f; // Farenin dikey hızı
    public float yMinLimit = -20f; // Dikey açının minimum limiti (aşağı bakma)
    public float yMaxLimit = 80f; // Dikey açının maksimum limiti (yukarı bakma)

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float distanceMin = 2f;
    public float distanceMax = 15f;

    [Header("Collision")]
    public bool cameraCollision = true;
    public float collisionPadding = 0.3f;
    public LayerMask collisionMask;

    // --- Private Değişkenler ---
    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Varsayılan olarak oyun modunda imleci kilitle
        LockCursor();
        Debug.Log("Cursor Lock State set to: " + Cursor.lockState);
    }

    void LateUpdate()
    {
        if (target)
        {
            // Sadece imleç kilitliyken kamerayı döndür
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // Zoom mesafesini ayarla
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            // Kamera çarpışma kontrolü
            if (cameraCollision)
            {
                RaycastHit hit;
                if (Physics.Linecast(target.position, position, out hit, collisionMask))
                {
                    // Eğer ışın bir engele çarparsa, kameranın mesafesini çarpışma noktasına göre ayarla
                    float newDistance = Vector3.Distance(target.position, hit.point) - collisionPadding;
                    // Zoom mesafesini geçici olarak küçült, ama kalıcı 'distance' değişkenini bozma
                    Vector3 tempNegDistance = new Vector3(0.0f, 0.0f, -newDistance);
                    position = rotation * tempNegDistance + target.position;
                }
            }
            
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // İmleci kilitleyen ve gizleyen public fonksiyon
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // İmleci serbest bırakan ve görünür kılan public fonksiyon
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}