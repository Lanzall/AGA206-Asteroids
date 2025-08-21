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
    public int PwrBeamStrength = 25;
    public float PwrBeamSpeed = 25f;

    [Header("References")]
    public Transform FirePoint;
    public GameObject BulletPrefab;

    private Rigidbody2D rb2D;
    private Vector2 moveInput;


    void Start()
    {
       CurrentPlayerState = PlayerState.Regular;
       rb2D = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
      
    }

    #region Movement
    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (CurrentPlayerState != PlayerState.Regular)
            return;

       if (CurrentPlayerState == PlayerState.Regular)
        {
            rb2D.linearVelocity = moveInput * Speed;
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

            if (CurrentPlayerState != PlayerState.Aiming)
                return;
        }

        else if (context.canceled)
        {
            CurrentPlayerState = PlayerState.Regular;

            if (moveInput != Vector2.zero)
            {
                rb2D.linearVelocity = moveInput * Speed;
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