using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    [Range(1,10)][SerializeField] private float zoomSpeed;
    [Range(1f, 5f)][SerializeField] private float minZoom;
    [Range(5f, 15f)][SerializeField] private float maxZoom;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            offset.y -= scroll * zoomSpeed;
            offset.y = Mathf.Clamp(offset.y, minZoom, maxZoom);
        }

        transform.position = player.position + offset;
    }
}
