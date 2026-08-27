using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] Vector3 closedRotation;
    [SerializeField] Vector3 openRotation;
    [SerializeField] float rotationSpeed;

    Quaternion targetRotation;


    bool isDoorOpen;
    void Start()
    {
        targetRotation = Quaternion.Euler(closedRotation);
        //transform.localRotation = targetRotation;
    }

    void Update()
    {
        if(Input.GetButtonDown("Use"))
        {
            if(isDoorOpen == false)
            {
                openDoor();
            }
            else
            {
                closeDoor();
            }
        }
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    void openDoor()
    {
        targetRotation = Quaternion.Euler(openRotation);
        isDoorOpen = true;
    }
    void closeDoor()
    {
        targetRotation = Quaternion.Euler(closedRotation);
        isDoorOpen = false;
    }

}
