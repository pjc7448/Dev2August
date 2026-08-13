using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{

    [Header("Weapon Settings")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPosition;
    [SerializeField] float ShootRate;
    [SerializeField] int BulletCount;
    [SerializeField] float BulletDecay;
    [SerializeField] float BulletSpeed;
    [SerializeField] float BulletSpread;
    [SerializeField] AudioSource GunShotSound;

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
            Vector3 direction = ShootPosition.forward;

            ShootTimer = 0; // reset the timer

            // Gun Sound effect
            if (GunShotSound!= null)
            {
                GunShotSound.Play();
            }


            for (int i = 0; i < BulletCount; i++)
            {
                float spread = Random.Range(-BulletSpread, BulletSpread);

                // Bullet only spread left to right (from the guns bullet origin)
                Vector3 spreadDirection = direction + ShootPosition.right * spread;

                spreadDirection.Normalize();

                Quaternion bulletRotation = Quaternion.LookRotation(spreadDirection);

                GameObject GunBullet = Instantiate(Bullet, ShootPosition.position, bulletRotation);

                Rigidbody rb = GunBullet.GetComponent<Rigidbody>();

                rb.linearVelocity = GunBullet.transform.forward * BulletSpeed;

                Destroy(GunBullet, BulletDecay);
            }
        }
    }
}
