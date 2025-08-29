using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Regular, Aiming, Death, Cutscene
}

public class ATPlayer : MonoBehaviour
{
    [Header("Player States")]
    public PlayerState CurrentPlayerState;

    [Header("Player Stats")]
    public float Speed = 5.0f;
    public float RotationSpeed = 5f;
    public int PwrBeamStrength = 25;
    public float PwrBeamSpeed = 25f;
    public float PSize = 2.5f;

    [Header("References")]
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public Animator baseAnimator;
    public Animator lightsAnimator;
    public Transform spriteHolder;

    private Rigidbody2D rb2D;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private float facingDirection;
    private float targetAngle;
    
    





    void Start()
    {
        CurrentPlayerState = PlayerState.Regular;
        rb2D = GetComponent<Rigidbody2D>();

        baseAnimator = transform.Find("SpriteHolder").GetComponent<Animator>();
        lightsAnimator = transform.Find("BodyLightsHolder").GetComponent<Animator>();

        transform.localScale = new Vector3(PSize, PSize, PSize);

        

        Debug.Log("Base Animator: " + baseAnimator.gameObject.name);
        Debug.Log("Lights Animator: " + lightsAnimator.gameObject.name);
    }

    void Update()
    {
      if (Input.GetKey(KeyCode.Space))
        {
            baseAnimator.SetBool("hasWeapon", true);
            lightsAnimator.SetBool("hasWeapon", true);


        }

        Rotation();
    }

    #region Movement
    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();       // Gets the movement direction
        targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;        // Gets the pointed angle in degrees

        if (CurrentPlayerState != PlayerState.Aiming)
        {
            baseAnimator.SetBool("isMoving", true);
            lightsAnimator.SetBool("isMoving", true);

            baseAnimator.SetFloat("InputX", moveInput.x);
            baseAnimator.SetFloat("InputY", moveInput.y);
            lightsAnimator.SetFloat("InputX", moveInput.x);
            lightsAnimator.SetFloat("InputY", moveInput.y);

            if (context.canceled)
            {
                baseAnimator.SetBool("isMoving", false);
                lightsAnimator.SetBool("isMoving", false);
            }

            if (CurrentPlayerState != PlayerState.Regular)
                return;

            if (CurrentPlayerState == PlayerState.Regular)
            {
                rb2D.linearVelocity = moveInput * Speed;
                //Flip();
            }

        }

        

        

    }

    /*public void Rotation()
    {
        if (CurrentPlayerState != PlayerState.Aiming)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            spriteHolder.rotation = Quaternion.Lerp(spriteHolder.rotation, targetRotation, Time.deltaTime * RotationSpeed);       // Rotates the player to the input direction smoothly,
                                                                                                                            // took a bit of research I don't understand Quaternions

        }

        if (moveInput == Vector2.zero)
        {
            spriteHolder.rotation = Quaternion.Euler(0, 0, 0);        // Snaps the player back to no rotation while Idle
        }
    }*/

    public void Rotation()
    {
        if (CurrentPlayerState != PlayerState.Aiming)
        {
            // Only rotate if there's movement input
            if (moveInput != Vector2.zero)
            {
                // Calculate angle from input
                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

                // Determine if angle is within horizontal or diagonal range
                bool isHorizontalOrDiagonal =
                    (Mathf.Abs(angle) <= 45f) ||         // Right and slight diagonals
                    (Mathf.Abs(angle) >= 135f);          // Left and slight diagonals

                if (isHorizontalOrDiagonal)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                    spriteHolder.rotation = Quaternion.Lerp(spriteHolder.rotation, targetRotation, Time.deltaTime * RotationSpeed);
                }
                else
                {
                    // Snap to upright for vertical movement
                    spriteHolder.rotation = Quaternion.identity;
                }
            }
            else
            {
                // No movement — reset rotation
                spriteHolder.rotation = Quaternion.identity;
            }
        }
    }


    public void Flip()      // Changes Player Sprite to face appropriate Direction
    {
        if (moveInput.x > 0.1f)
        {
            facingDirection = PSize;
        }
        else if (moveInput.x < -0.1f)
        {
            facingDirection = -PSize;
        }

        transform.localScale = new Vector3(facingDirection, PSize, PSize);
    }

    #endregion

    #region Combat

    public void MousePosition(InputAction.CallbackContext context)
    {
        if (CurrentPlayerState != PlayerState.Aiming)
            return;

        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();
        Debug.Log("Mouse Screen Position" + mouseScreenPosition);
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CurrentPlayerState = PlayerState.Aiming;
            rb2D.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            baseAnimator.SetBool("isMoving", false);
            lightsAnimator.SetBool("isMoving", false);

            baseAnimator.SetBool("isAiming", true);
            lightsAnimator.SetBool("isAiming", true);

            if (CurrentPlayerState != PlayerState.Aiming)
                return;
        }

        else if (context.canceled)
        {
            CurrentPlayerState = PlayerState.Regular;
            baseAnimator.SetBool("isAiming", false);
            lightsAnimator.SetBool("isAiming", false);

            if (moveInput != Vector2.zero)              //If movement is still being held after releasing aim, resume movement, play animation and adjust the facing direction
            {
                baseAnimator.SetBool("isMoving", true);
                lightsAnimator.SetBool("isMoving", true);

                rb2D.linearVelocity = moveInput * Speed;
                Flip();
            }

        }
        
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (CurrentPlayerState != PlayerState.Aiming)
            return;

        if (context.performed)
        {
            PowerBeam();
        }
    }

    void PowerBeam()
    {
        GameObject powerbeam = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = powerbeam.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * PwrBeamSpeed;
    }

    #endregion
}


/*
 * Hold down aim button, enters aiming state - locks player movement and makes reticle / bounds appear in front showing where they can aim
 * direction pointed with left stick or mouse position detirmines the direction of the attack
 * Mouse - find coordinates relative to players position on the screen
 * releasing Aim returns player to normal movement
 * draws a line on the targeting arc to show where the character is aiming, also moves characters arms and gun to face this direction
 * while aiming, can press shoot button to fire a beam attack
 * 
 * Define the aim button and setup the input
 * Setup a bool flag (Or Enum) to register when we are aiming
 * If we are aiming, lock players directional movement, and enable aiming controls
 * Get position of the mouse screen space
 * 
 * 
 */