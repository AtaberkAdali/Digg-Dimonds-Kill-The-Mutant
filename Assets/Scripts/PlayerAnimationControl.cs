using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    Animator animator;
    public Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void WalkAnim()
    {
        Debug.Log("ileri yürüdüm.");
        animator.SetBool("Walk", true);
    }
    public void IdleAnim()
    {
        Debug.Log("idle.");
        animator.SetBool("Walk", false);
    }
    public void AttackAnim()
    {
        Debug.Log("attack.");
        animator.SetBool("Attack", true);
    }
    public void FinishAttackAnim()
    {
        animator.SetBool("Attack", false);
    }
    public void DanceAnim()
    {
        animator.SetBool("EnemyDie", true);
    }


    public void EnemyIdleAnim()
    {
        enemyAnimator.SetBool("darbeAldi", false);
    }
    public void EnemyDarbeAnim()
    {
        enemyAnimator.SetBool("darbeAldi", true);
    }
    public void EnemydieAnim()
    {
        enemyAnimator.SetBool("die", true);
    }
}
