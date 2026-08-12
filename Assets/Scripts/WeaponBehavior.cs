using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{

    [Header("Weapon Settings")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPosition;
    [SerializeField] float ShootRate;
    [SerializeField] int BulletCount;
    [SerializeField] float BulletSpread;

    float ShootTimer;

    // Update is called once per frame
    void Update()
    {
        ShootTimer += Time.deltaTime;   // for the weapon fire rate
    }

    public void Shoot()
    {
        if (ShootTimer >= ShootRate)
        {
            // this is so weapons shoot at the players retical, this will be changed when we implement the top down camera (or removed)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 CrossHair;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                CrossHair = hit.point;
            }
            else
            {
                CrossHair = ray.origin + ray.direction * 1000f;
            }

            Vector3 direction = CrossHair - ShootPosition.position;
            direction.y = 0;
            direction.Normalize();
            ShootTimer = 0; // reset the timer

            for (int i = 0; i < BulletCount; i++)
            {
                float spread = Random.Range(-BulletSpread, BulletSpread);

                // Bullet only spread left to right (from the guns bullet origin)
                Vector3 spreadDirection = direction + ShootPosition.right * spread;

                spreadDirection.Normalize();

                Quaternion bulletRotation = Quaternion.LookRotation(spreadDirection);

                GameObject GunBullet = Instantiate(Bullet, ShootPosition.position, bulletRotation);
            }
        }
    }
}
