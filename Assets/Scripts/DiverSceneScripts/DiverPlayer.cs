using UnityEngine;
using UnityEngine.InputSystem;

public class DiverPlayer : MonoBehaviour
{
    

    public float moveSpeed = 1f;

    private Vector2 moveInput;             //Variable storing the amount the movement is used? Probably won't need the full in betweens when pushing softly, change if needed

    private Rigidbody2D rb2D;

    

    private void Awake()
    {

        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2D.linearVelocity = moveInput * moveSpeed;
    }


    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
