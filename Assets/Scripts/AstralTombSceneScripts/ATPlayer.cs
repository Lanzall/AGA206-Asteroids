using System.Collections;
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
    public int HealthMax = 3;
    public int HealthCurrent;

    public float Speed = 5.0f;
    public float RotationSpeed = 5f;

    public float PwrBeamSpeed = 25f;
    public float RocketSpeed = 15f;

    public float InvincibilityDuration = 1f;

    [Header("References")]
    public Transform FirePoint;
    public GameObject PBeamPrefab;
    public GameObject RocketPrefab;
    public Animator baseAnimator;
    public Animator lightsAnimator;
    public Transform spriteHolder;
    public GameObject Arms;
    public GameObject ArmsLights;



    private Rigidbody2D rb2D;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private float targetAngle;
    private Quaternion targetRotation;
    private float angleDifference;
    private bool AnimMoving;
    private SpriteRenderer baseSprite;
    private SpriteRenderer lightsSprite;
    private SpriteRenderer armsSprite;
    private SpriteRenderer armsLightsSprite;
    private bool TakesDamage;
    private Material healthMat;
    private bool isInvincible;
    private bool RocketToggle;
    private bool hasWeapon;
    
    







    void Start()
    {
        CurrentPlayerState = PlayerState.Regular;
        rb2D = GetComponent<Rigidbody2D>();

        baseAnimator = transform.Find("SpriteHolder").GetComponent<Animator>();
        lightsAnimator = transform.Find("SpriteHolder/BodyLightsHolder").GetComponent<Animator>();

        baseSprite = transform.Find("SpriteHolder").GetComponent<SpriteRenderer>();
        lightsSprite = transform.Find("SpriteHolder/BodyLightsHolder").GetComponent<SpriteRenderer>();

        armsSprite = transform.Find("Arms").GetComponent<SpriteRenderer>();
        armsLightsSprite = transform.Find("Arms/ArmsLights").GetComponent<SpriteRenderer>();

        HealthCurrent = HealthMax;
        healthMat = lightsSprite.material;
        isInvincible = false;
        RocketToggle = false;
        hasWeapon = false;

        Arms.SetActive(false);
        ArmsLights.SetActive(false);

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
            hasWeapon = true;


        }

        if (AnimMoving == true)
        {
            Debug.Log("Moving");
            
        }

        Rotation();
        RotationChecker();

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(25);
        }
    }





    #region Player Health
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        baseAnimator.SetTrigger("hurt");

        HealthCurrent = HealthCurrent - damage;
        StartCoroutine(ActivateInvincibility());



        if (HealthCurrent <= 50)
        {
            
        }
    }



    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        StartCoroutine(FlashDuringInvincibility());
        yield return new WaitForSeconds(InvincibilityDuration);
        isInvincible = false;
    }

    IEnumerator FlashDuringInvincibility()
    {
        float elapsed = 0f;
        while (elapsed < InvincibilityDuration)
        {
            baseSprite.enabled = !baseSprite.enabled;
            lightsSprite.enabled = !lightsSprite.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        baseSprite.enabled = true;
        lightsSprite.enabled = true;
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
        //if (CurrentPlayerState != PlayerState.Aiming)
           // return;

        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();     //Gets mouse position
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);       //Converts it to world space
        mouseWorldPosition.z = 0f;

        Vector3 directionToMouse = (mouseWorldPosition - Arms.transform.position).normalized;       //Finds the direction from arms to mouse

        bool mouseIsBehind = (mouseWorldPosition.x < transform.position.x);     //Checks if the mouse is behind the player

       if (CurrentPlayerState == PlayerState.Aiming)
        {
            baseSprite.flipX = mouseIsBehind;       //flips sprites only while aiming
            lightsSprite.flipX = mouseIsBehind;
            armsSprite.flipX = mouseIsBehind;
            armsLightsSprite.flipX = mouseIsBehind;
            FirePoint.transform.rotation = Arms.transform.rotation;
        }



        float baseAngle = mouseIsBehind ? 180f : 0f;        //flips aiming bounds
        Vector3 baseForward = mouseIsBehind ? -transform.right : transform.right;

        float angleToMouse = Vector3.SignedAngle(baseForward, directionToMouse, Vector3.forward);       //Angle between base direction and mouse direction

        float clampedAngle = Mathf.Clamp(angleToMouse, -30f, 30f);        //Clamps the angle in 60 degrees

        Arms.transform.rotation = Quaternion.Euler(0f, 0f, clampedAngle);       //Rotates the Arms
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (hasWeapon == false)
            return;

        if (context.performed)
        {
            Arms.SetActive(true);
            ArmsLights.SetActive(true);

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
            Arms.SetActive(false);
            ArmsLights.SetActive(false);

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

    

    public void RocketEnable(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RocketToggle = true;
        }

        else if (context.canceled)
        {
            RocketToggle = false;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (CurrentPlayerState != PlayerState.Aiming)
            return;

        if ((context.performed) && RocketToggle == false)
        {
            PowerBeam();
        }

        else if ((context.performed && RocketToggle == true))
        {
            Rocket();
        }
    }

    void PowerBeam()
    {
        GameObject powerbeam = Instantiate(PBeamPrefab, FirePoint.position, Arms.transform.rotation);
        Rigidbody2D rb = powerbeam.GetComponent<Rigidbody2D>();

        if (armsSprite.flipX == false)
        {
            rb.linearVelocity = Arms.transform.right * PwrBeamSpeed;
        }

        else if (armsSprite.flipX == true)
        {
            
            rb.linearVelocity = -FirePoint.right * PwrBeamSpeed;
        }

    }

    void Rocket()
    {
        GameObject rocket = Instantiate(RocketPrefab, FirePoint.position, Arms.transform.rotation);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        if (armsSprite.flipX == false)
        {
            rb.linearVelocity = Arms.transform.right * RocketSpeed;
        }

        else if (armsSprite.flipX == true)
        {

            rb.linearVelocity = -FirePoint.right * RocketSpeed;
        }
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