using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 10, -8);
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        transform.position = player.position + cameraOffset;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}

