using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraOffset = new Vector3(0, 15, 0);


    float camRotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + cameraOffset;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}

