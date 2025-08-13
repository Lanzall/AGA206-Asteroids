using UnityEngine;
using UnityEngine.InputSystem;

public class ATPlayer : MonoBehaviour
{
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
       rb2D = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        rb2D.linearVelocity = moveInput * Speed;
    }

    #region Movement
    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    #endregion

    #region Combat
    public void Shoot(InputAction.CallbackContext context)
    {
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
