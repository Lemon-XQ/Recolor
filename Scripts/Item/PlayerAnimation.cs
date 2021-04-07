using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;

	void Start () {
        animator = this.GetComponent<Animator>();
	}

    public void IdleState()
    {
        animator.SetBool("Die", false);
        animator.SetInteger("direction", 0);
        //animator.SetFloat("Run", 0.5f);
    }

    public void RunState(int direction)
    {
        //float value = isRunL ? 0 : 1;
        animator.SetInteger("direction", direction);

    }

    public void JumpState(bool isJump/*,bool left*/)
    {
        if (isJump)
        {
            animator.SetBool("jump", true);

            //if (left)
            //    animator.SetBool("JumpL", true);
            //else
            //    animator.SetBool("JumpR", true);
            //AudioMgr.Instance.PlayMusic("Audio_jump");
        }
        else
        {
            animator.SetBool("jump", false);
            //animator.SetBool("JumpL", false);
            //animator.SetBool("JumpR", false);
        }
    }

    public void ClimbState(bool isClimbing)
    {
        if (isClimbing)
        {
            animator.SetBool("Climb", true);
        }
        else
        {
            animator.SetBool("Climb", false);
        }
    }
    public void DieState()
    {
        animator.SetBool("Die", true);
    }
}
