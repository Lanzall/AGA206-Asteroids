using System.Collections;
using UnityEngine;

public class CockpitHUD : MonoBehaviour
{
    public float DamageTime = .5f;
    public float RoidDestroyTime = .5f;

    private Spaceship ship;
    private Animator animator;
    void Start()
    {
        ship = FindFirstObjectByType<Spaceship>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator TakenDamage()
    {
        animator.SetTrigger("Damaged");
        yield return new WaitForSeconds(DamageTime);
        Idle();
        yield return null;
    }

    public IEnumerator RoidDestroy()
    {
        animator.SetTrigger("AsteroidDestroy");
        yield return new WaitForSeconds(RoidDestroyTime);
        Idle();
        Debug.Log("RoidTimeOver");
        yield return null;
    }

    public IEnumerator PlayerDeath()
    {
        animator.SetTrigger("Death");
        yield return null;
    }

    void Idle()
    {
        animator.SetTrigger("ReturnToIdle");
    }
}
