using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;

    [Header("Stats")]
    [Range(0, 10)][SerializeField] int HP;
    [SerializeField] int FaceTargetSpeed;



    [Header("Weapons")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform GunPivot;
    [SerializeField] Transform ShootPosition;
    [SerializeField] float ShootRate;
    [SerializeField] int GunRotationSpeed;
    [SerializeField] float BulletSpeed;
    [SerializeField] float BulletDecay;
    [SerializeField] AudioSource ShootSound;

    Color colorOrig;

    Vector3 PlayerDir;

    float ShootTimer;

    bool PlayerInTrigger;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.Instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    { 

        if (PlayerInTrigger)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position);
            ShootTimer += Time.deltaTime;
            PlayerDir = GameManager.Instance.player.transform.position - transform.position;
            FaceTarget();
            RotateGun();

            if (ShootTimer >= ShootRate)
            {
                ShootSound.Play();
                Shoot();
            }
        }
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(PlayerDir.x, 0, PlayerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, FaceTargetSpeed * Time.deltaTime);

    }

    void RotateGun()
    {
        Quaternion rot = Quaternion.LookRotation(PlayerDir);
        GunPivot.rotation = Quaternion.Lerp(GunPivot.rotation, rot, GunRotationSpeed * Time.deltaTime);
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void Shoot()
    {
        ShootTimer = 0;

        GameObject bullet = Instantiate(
            Bullet,
            ShootPosition.position,
            ShootPosition.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.linearVelocity = bullet.transform.forward * BulletSpeed;

        Destroy(bullet, BulletDecay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = false;
        }
    }

    public void takeDamage(int amount)
    {

        HP -= amount;

        if (HP <= 0)
        {
            GameManager.Instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }
}