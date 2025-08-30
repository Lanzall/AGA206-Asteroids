using Unity.VisualScripting;
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
    public int HealthMax = 100;
    public int HealthCurrent;
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
    private float targetAngle;
    private Quaternion targetRotation;
    private float angleDifference;
    private bool AnimMoving;
    private SpriteRenderer baseSprite;
    private SpriteRenderer lightsSprite;
    private bool TakesDamage;
    private Material healthMat;
    







    void Start()
    {
        CurrentPlayerState = PlayerState.Regular;
        rb2D = GetComponent<Rigidbody2D>();

        baseAnimator = transform.Find("SpriteHolder").GetComponent<Animator>();
        lightsAnimator = transform.Find("SpriteHolder/BodyLightsHolder").GetComponent<Animator>();
        

        baseSprite = transform.Find("SpriteHolder").GetComponent<SpriteRenderer>();
        lightsSprite = transform.Find("SpriteHolder/BodyLightsHolder").GetComponent<SpriteRenderer>();

        HealthCurrent = HealthMax;
        healthMat = lightsSprite.material;

        

        Debug.Log("Base Animator: " + baseAnimator.gameObject.name);
        Debug.Log("Lights Animator: " + lightsAnimator.gameObject.name);
    }

    void Update()
    {
        AnimMoving = baseAnimator.GetBool("isMoving");

        if (Input.GetKey(KeyCode.Space))
        {
            baseAnimator.SetBool("hasWeapon", true);
            lightsAnimator.SetBool("hasWeapon", true);


        }

        if (AnimMoving == true)
        {
            Debug.Log("Moving");
            
        }

        Rotation();
        RotationChecker();
    }

    #region Player Health
    public void TakeDamage()
    {
        baseAnimator.SetTrigger("hurt");

        if (HealthCurrent <= 50)
        {
            
        }
    }

    #endregion


    #region Movement
    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();       // Gets the movement direction

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
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (CurrentPlayerState != PlayerState.Regular)
                return;

            if (CurrentPlayerState == PlayerState.Regular)
            {
                rb2D.linearVelocity = moveInput.normalized * Speed;
                Flip();
            }

        }

    }

    public void RotationChecker()
    {
        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, 0, angle);
        angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
    }

    public void Rotation()
    {
        if (AnimMoving == true)
        {
            if (moveInput.x == -1)
            {
                transform.rotation = Quaternion.Euler(0, 0, -180f);         // Did this because the player wouldn't rotate from a neutral positon to face right
            }
            

            if (angleDifference < 160f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);        // Smooth rotation for all directions, only rotating if the angle difference is less than 180 degrees
            }

            else if (angleDifference < 10f)
            {
                transform.rotation = targetRotation;
                Debug.Log("SmallAngle");
            }
        }
    }

    public void Flip()      // Changes Player Sprite to face appropriate Direction
    {
        if (moveInput.x > 0.1f)
        {
            baseSprite.flipX = false;
            lightsSprite.flipX = false;
        }
        else if (moveInput.x < -0.1f)
        {
            baseSprite.flipX = true;
            lightsSprite.flipX = true;
        }
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