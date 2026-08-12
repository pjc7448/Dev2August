using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamage, IHeal
{
    [SerializeField] CharacterController controller;

    // The players Weapon
    [SerializeField] WeaponBehavior Weapon;

    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 6)][SerializeField] int Speed;
    [Range(2, 5)][SerializeField] int sprintMod;
    [Range(8, 20)][SerializeField] int jumpSpeed;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(15, 40)][SerializeField] int gravity;

    [SerializeField] LayerMask IgnoreLayer;


    int jumpCount;
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
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity.y = 0;
        }

        Vector3 cameraForward = Vector3.forward;
        Vector3 cameraRight = Vector3.right;

        moveDirection = Input.GetAxisRaw("Horizontal") * cameraRight + Input.GetAxisRaw("Vertical") * cameraForward;
        controller.Move(moveDirection.normalized * Speed * Time.deltaTime);

        Jump();
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

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

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;


        }
    }

    IEnumerator FlashDamage()
    {
        GameManager.Instance.damageFlashPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.damageFlashPanel.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        GameManager.Instance.playerHPBar.fillAmount = (float) HP / HPOriginal;
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

