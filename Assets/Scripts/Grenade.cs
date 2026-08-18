using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float fuseTime;
    [SerializeField] float explosionRadius;
    [SerializeField] int damageAmount;


    void Start()
    {
        Invoke("Explode", fuseTime);
    }

    void Explode()
    {
        Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider objectHit in objectsHit)
        {
            IDamage damageTarget = objectHit.GetComponent<IDamage>();

            if(damageTarget != null)
            {
                damageTarget.takeDamage(damageAmount);
            }
        }
        Destroy(gameObject);
    }

}
