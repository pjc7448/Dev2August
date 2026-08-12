using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] int sensitivity;
    [SerializeField] int LockVertMin, LockVertMax;

    float camRotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused == false)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

            camRotX -= mouseY;
            camRotX = Mathf.Clamp(camRotX, LockVertMin, LockVertMax);
            transform.localRotation = Quaternion.Euler(camRotX, 0, 0);

            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}

