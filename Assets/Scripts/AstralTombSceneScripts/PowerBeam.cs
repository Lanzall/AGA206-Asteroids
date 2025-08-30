using UnityEngine;

public class PowerBeam : MonoBehaviour
{
    public int Damage = 25;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;

        // Flip sprite if moving left
       /* if (velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
