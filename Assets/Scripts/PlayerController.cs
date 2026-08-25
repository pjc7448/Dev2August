using UnityEngine;
using System.Collections;


// DUe to repo issues with our team, Code was overwritten with code from the  lectures 
// Moteak did made his own Playercontroller different from the lecture but was lost on commits
// The credit for the Base code for the player controller (not dash or Ideal functions/ties) goes to Moteak, as its late into the project to incoprate
// exact work due to potential refernce issues and Conflictions.

public class PlayerController : MonoBehaviour, IDamage, IHeal
{
    [Header("Components")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask IgnoreLayer;

    [Header("Gun")]
    [SerializeField] WeaponBehavior Weapon;
    // The Weapon is a pickup
    [SerializeField] Transform WeaponDir;
    WeaponBehavior PickupRange;

    [Header("Player Audio")]
    [SerializeField] AudioSource PlayerDamageSound;
    [SerializeField] AudioSource PlayerWalkingSoud;
    [SerializeField] AudioSource WeaponEquipSound;

    [Header("Player Stats")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 6)][SerializeField] int Speed;
    [Range(2, 5)][SerializeField] int sprintMod;

    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [Header("Player Ability")]
    [SerializeField] GameObject grenade;
    [SerializeField] Transform throwPosition;
    [SerializeField] float throwForce;
    float dashTimer;
    bool isDashing;

   


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
        FaceMouse();

        if(Input.GetButtonDown("Throw"))
        {
            Throw();
        }
        

        dashTimer += Time.deltaTime;
        if (Input.GetButtonDown("Dash") && dashTimer >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // E is now used to pickup weapons
        if (PickupRange != null && Input.GetKeyDown(KeyCode.E))
        {
            EquipWeapon(PickupRange);
            PickupRange = null;
            GameManager.Instance.PickUpPrompt.gameObject.SetActive(false);
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
        GameManager.Instance.playerHPBar.fillAmount = (float)HP / HPOriginal;
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

        // Hit effect should sound off
        if (PlayerDamageSound != null)
        {
            PlayerDamageSound.Play();
        }

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

        if (WeaponEquipSound != null)
        {
            WeaponEquipSound.Play();
        }

        newWeapon.transform.SetParent(WeaponDir);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        // delete the collider to prevent the weapon from despawning (the player has the collider inside them and deletes it again)
        Transform rangecollider = newWeapon.transform.Find("PickupRange");

        if (rangecollider != null)
        {
            Destroy(rangecollider.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponBehavior newWeapon = other.GetComponentInParent<WeaponBehavior>();

        if (newWeapon != null)
        {
            PickupRange = newWeapon;
            GameManager.Instance.PickUpPrompt.text = "Equip: " + newWeapon.GetWeaponName();
            GameManager.Instance.PickUpPrompt.gameObject.SetActive(true);
        }
    }

    // this is sets the pickup to null to prevent the gun from being remebered (can pickup after leaving thr trigger range)
    private void OnTriggerExit(Collider other)
    {
        WeaponBehavior newWeapon = other.GetComponentInParent<WeaponBehavior>();

        if (newWeapon != null && PickupRange == newWeapon)
        {
            PickupRange = null;
            GameManager.Instance.PickUpPrompt.gameObject.SetActive(false);
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

    void FaceMouse()
    {
        // a ray will shoot from the camera to the mouse on screen.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit mouse, 1000f))
        {
            // this gets the players direction to face the mouse

            Vector3 Direction = mouse.point - transform.position;

            Direction.y = 0;

            if (Direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(Direction);
            }
        }
    }
    void Throw()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit mouse, 1000f))
        {
            Vector3 direction = mouse.point - throwPosition.transform.position;
            direction.Normalize();

            GameObject Grenade = Instantiate(grenade, throwPosition.transform.position, Quaternion.identity);

            Rigidbody rb = Grenade.GetComponent<Rigidbody>(); ;

            rb.linearVelocity = direction * throwForce;
        }
    }

    public void SpawnPlayer()
    {
        controller.transform.position = GameManager.Instance.spawnPosition.transform.position;
    }
}

