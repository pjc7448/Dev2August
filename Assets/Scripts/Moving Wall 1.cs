using UnityEngine;

public class MovingWall1 : MonoBehaviour
{

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] int moveSpeed;

    Vector3 pointAPosition;
    Vector3 pointBPosition;
    Vector3 target;
    void Start()
    {
        pointAPosition = pointA.position;
        pointBPosition = pointB.position;

        target = pointBPosition;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (target == pointBPosition)
            {
                target = pointAPosition;
            }
            else
            {
                target = pointBPosition;
            }
        }
    }
}
