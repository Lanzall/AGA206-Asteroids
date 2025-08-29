using System.Collections;
using UnityEngine;

public class CockpitHUD : MonoBehaviour
{
    public float DamageTime = .5f;
    public float RoidDestroyTime = .5f;

    private SpriteRenderer SpriteRenderer;
    private Color originalColor;

    private Spaceship ship;
    private Animator animator;
    

    void Start()
    {
        ship = FindFirstObjectByType<Spaceship>();
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = SpriteRenderer.color;   //Storing the original color
    }

    public IEnumerator TakenDamage()
    {
        animator.SetTrigger("Damaged");
        SpriteRenderer.color = Color.red;   //Changes to red
        yield return new WaitForSeconds(DamageTime);
        SpriteRenderer.color = originalColor;   //Changes to original color
        Debug.Log("ReturningToIdleFromDamage");
        Idle();
        yield return null;
    }

    public IEnumerator RoidDestroy()
    {
        animator.SetTrigger("RoidDestroy");
        yield return new WaitForSeconds(RoidDestroyTime);
        Idle();
        Debug.Log("RoidTimeOver");
        yield return null;
    }

    public IEnumerator PlayerDeath()
    {
        animator.SetTrigger("Death");
        SpriteRenderer.color = Color.red;
        yield return null;
    }

    void Idle()
    {
        animator.SetTrigger("ReturnToIdle");
    }
}
