// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // Karakterin dönüş hızı (isteğe bağlı)

    private CharacterController characterController;
    private Camera mainCamera; // Kamerayı referans almak için

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Sahnedeki ana kamerayı bul
    }

    void Update()
    {
        // Input al
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D veya Sol/Sağ Ok
        float verticalInput = Input.GetAxis("Vertical");   // W/S veya Yukarı/Aşağı Ok

        // Hareket vektörünü oluştur (kamera yönüne göre)
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // Y eksenindeki hareketi dikkate alma (kameranın eğiminden etkilenmemek için)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        moveDirection.Normalize(); // Çapraz harekette hızı sabit tutmak için

        // Karakteri hareket ettir
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Karakteri hareket yönüne döndür (isteğe bağlı, daha iyi hissettirir)
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}