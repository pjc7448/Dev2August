using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamage, IHeal
{
    [SerializeField] CharacterController controller;

    // The players Weapon
    [SerializeField] WeaponBehavior Weapon;
    // The Weapon is a pickup
    [SerializeField] Transform WeaponDir;

    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 6)][SerializeField] int Speed;
    [Range(2, 5)][SerializeField] int sprintMod;

    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;
    float dashTimer;
    bool isDashing;

    [SerializeField] LayerMask IgnoreLayer;


    int HPOriginal;

    Vector3 moveDirection;
    Vector3 playerVelocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;     // when dies, hp is reset back to orignal
        UpdatePlayerUI();

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        Sprint();

        dashTimer += Time.deltaTime;
        if (Input.GetButtonDown("Dash") && dashTimer >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void movement()
    {
        if(isDashing)
        {
            return;
        }

        Vector3 cameraForward = Vector3.forward;
        Vector3 cameraRight = Vector3.right;

        moveDirection = Input.GetAxisRaw("Horizontal") * cameraRight + Input.GetAxisRaw("Vertical") * cameraForward;
        controller.Move(moveDirection.normalized * Speed * Time.deltaTime);

        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            Weapon.Shoot();
        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            Speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            Speed /= sprintMod;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashTimer = 0;

        Vector3 dashDirection = moveDirection;
        if(dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        float dashTime = 0;

        while(dashTime < dashDuration)
        {
            controller.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);

            dashTime += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
    }

    IEnumerator FlashDamage()
    {
        GameManager.Instance.damageFlashPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.damageFlashPanel.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        Heal heal = GetComponent<Heal>();
        if(heal != null)
        {
            heal.damageTaken();
        }
        UpdatePlayerUI();
        StartCoroutine(FlashDamage());

        if (HP <= 0)
        {
            GameManager.Instance.youLose();
        }
    }
    public void EquipWeapon(WeaponBehavior newWeapon)
    {
        if (Weapon != null)
        {
            Destroy(Weapon.gameObject);
        }

        Weapon = newWeapon;

        newWeapon.transform.SetParent(WeaponDir);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

    }
    private void OnTriggerEnter(Collider other)
    {
        // to see if the gun gets picked up
        Debug.Log("TRIGGER: " + other.name);

        WeaponBehavior newWeapon = other.GetComponentInParent<WeaponBehavior>();

        if (newWeapon != null)
        {
            Debug.Log("WEAPON FOUND: " + newWeapon.name);
            EquipWeapon(newWeapon);
        }
    }

    public void heal(int amount)
    {
        HP += amount;
        if(HP > HPOriginal)
        {
            HP = HPOriginal;
        }
        UpdatePlayerUI();
    }
    public bool isFullHealth()
    {
        return HP >= HPOriginal;
    }
}

