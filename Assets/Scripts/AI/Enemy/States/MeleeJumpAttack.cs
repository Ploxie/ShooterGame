using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/JumpAttack")]

public class MeleeJumpAttack : State
{
    protected EnemyMelee enemyMelee;

    protected Assets.Scripts.Entity.Player player;

    protected float baseSpeed;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemyMelee = (EnemyMelee)parent;
        player = enemyMelee.Player;
        baseSpeed = enemyMelee.Agent.speed;
    }

    public override void ChangeState()
    {
        if (enemyMelee.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            float distance = Vector3.Distance(player.transform.position, enemyMelee.transform.position);
            if (distance <= enemyMelee.AttackRange)
            {
                enemyMelee.StateMachine.SetState(typeof(MeleeAttack));
            }
            else if (distance > enemyMelee.AttackRange)
            {
                enemyMelee.StateMachine.SetState(typeof(MeleeRunToPlayer));
            }
        }
    }

    public override void Enter()
    {
        enemyMelee.Agent.SetDestination(player.transform.position);
        enemyMelee.Animator.SetBool("IsWalking", false);
        enemyMelee.Animator.SetBool("IsJumpAttacking", true);
        enemyMelee.Agent.speed += baseSpeed / 2;
    }
    public override void Exit()
    {
        enemyMelee.Agent.speed -= baseSpeed / 2;
        enemyMelee.Animator.SetBool("IsJumpAttacking", false);
    }

    public override void Update()
    {
        
    }
}
