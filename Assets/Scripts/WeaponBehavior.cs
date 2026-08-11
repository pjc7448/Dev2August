using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{

    [Header("Weapon Settings")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPosition;
    [SerializeField] float ShootRate;
    [SerializeField] float BulletDecay;
    [SerializeField] float BulletSpeed;

    float ShootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootTimer += Time.deltaTime;   // for the weapon fire rate
    }

    public void Shoot()
    {
        if (ShootTimer >= ShootRate)
        {
            ShootTimer = 0; // reset the timer
            GameObject GunBullet = Instantiate(Bullet, ShootPosition.position, ShootPosition.rotation);

            Rigidbody rb = GunBullet.GetComponent<Rigidbody>();

            rb.linearVelocity = GunBullet.transform.forward * BulletSpeed;

            Destroy(GunBullet, BulletDecay);
        }
    }
}
