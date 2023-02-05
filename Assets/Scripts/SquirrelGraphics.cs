using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelGraphics : MonoBehaviour
{
    private Animator animator;

    private enum AnimationState { 
        Idle = 0,
        Run = 1,
        Die = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        animator.SetInteger("State", (int)AnimationState.Idle);
    }

    public void Run()
    {
        animator.SetInteger("State", (int)AnimationState.Run);
    }
    public void Die()
    {
        animator.SetInteger("State", (int)AnimationState.Die);
    }

    public void Fire()
    {
        animator.Play("Fire");
    }
}
