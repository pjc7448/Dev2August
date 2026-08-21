using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{

    enum DamageType { Bullet, Stationary, DOT };
    [SerializeField] DamageType type;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int bulletDestroyTime;
    [SerializeField] ParticleSystem hitEffect;

    bool isDamaging;


    void Start()
    {
        if (type == DamageType.Bullet)
        {
            Destroy(gameObject, bulletDestroyTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type != DamageType.DOT)
        {
            dmg.takeDamage(damageAmount);
        }

        if (type == DamageType.Bullet)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == DamageType.DOT && !isDamaging)
        {
            StartCoroutine(damageOther(dmg));

        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}

