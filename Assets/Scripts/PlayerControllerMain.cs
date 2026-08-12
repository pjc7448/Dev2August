using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController control;

    [Range(1, 100)][SerializeField] int playerHP;
    [Range(1, 10)][SerializeField] int playerSpeed;
    [Range(1, 2)][SerializeField] int dashMod;
    [Range(1, 3)][SerializeField] int dashDuration;
    [Range(1, 5)][SerializeField] int dashCoolDown;
    [Range(15, 50)][SerializeField] int yGravity;

    int playerHpOrig;
    float dashTimer;
    bool dashing;


    Vector3 playerDir;
    Vector3 playerVel;
    
    void Start()
    {
        int playerHpOrig = playerHP;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Dash();
    }

    void Movement()
    {
        // gets the player horizontal and vertical inputs, horizontal controls left and right movements while vertical does forward and back
        playerDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        //Moves player in the direction they are pressing and keeps the diagnol speed form being faster than normal movemnets
        // also keeps the movement the same regradless of fps
        control.Move(playerDir.normalized * playerSpeed * Time.deltaTime);

        //checks to see if the player is touching the ground and keeps gravity from building up
        if(control.isGrounded && playerVelocity.y <= 0)
        {
            playerVelocity.y = -2f;
        }

        //adds gravity to the players y velocity 
        playerVelocity.y += yGravity * Time.deltaTime;

        // moves the players velocity using the falling velocity
        control.Move(playerVelocity * Time.deltaTime);
    }

    void Dash()
    {
        //This is asking if play pushed the button and if they are currently not dashing
        if (Input.GetButtonDown("Dash") && dashing == false)
        {
            // if both conditions above are met then the player starts the dash
            dashing = true;
            // this line starts the count for how long the player will dash for and it will end once the count is up
            dashTimer = dashDuration;
            // this line takes the players speed and then multiplies by the dashMod.
            playerSpeed *= dashMod;
            
        }
        // only do the code below if while the player is dashing
        if (dashing == true)
        {
            //subtracts the amount of time that has passed during this frame from the timer
            dashTimer -= Time.deltaTime;
            
            // once the dash timer hits 0 or less end the dash 
            if(dashTimer <= 0)
            {
                dashing = false;

                // divde the player speed by the dashMod amount
                playerSpeed /= dashMod;
            }
        }
    }
}
