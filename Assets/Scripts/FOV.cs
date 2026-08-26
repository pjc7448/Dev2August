using UnityEngine;

public class FOV : MonoBehaviour
{
    EnemyAI enemy;
    Mesh FOVMesh;
    void Start()
    {
        enemy = GetComponentInParent<EnemyAI>();
        FOVMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = FOVMesh;

        FOVCone();
    }

    void Update()
    {

    }


    void FOVCone()
    {
        Vector3 leftEdge = Quaternion.Euler(0, -enemy.ViewAngle / 2, 0) * Vector3.forward;
        Vector3 rightEdge = Quaternion.Euler(0, enemy.ViewAngle / 2, 0) * Vector3.forward;

        Vector3 leftPoint = leftEdge * enemy.ViewDistance;
        Vector3 rightPoint = rightEdge * enemy.ViewDistance;

        FOVMesh.vertices = new Vector3[] { Vector3.zero, leftPoint, rightPoint };
        FOVMesh.triangles = new int[] { 0, 1, 2 };

    }
}
