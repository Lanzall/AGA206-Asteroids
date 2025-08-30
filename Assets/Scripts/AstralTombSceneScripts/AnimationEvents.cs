using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HurtAnimationEnded()
    {
        animator.SetBool("hurt", false);
    }
 
}
