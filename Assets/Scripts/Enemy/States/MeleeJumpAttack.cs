using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/JumpAttack")]

public class MeleeJumpAttack : State
{
    EnemyMelee enemy;

    Player player;

    float baseSpeed;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyMelee)parent;
        player = enemy.Player;
        baseSpeed = enemy.Agent.speed;
    }
    public override void ChangeState()
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance <= enemy.AttackRange)
            {
                enemy.StateMachine.SetState(typeof(MeleeAttack));
            }
            else if (distance > enemy.AttackRange)
            {
                enemy.StateMachine.SetState(typeof(MeleeRunToPlayer));
            }

        }
    }

    public override void Enter()
    {
        enemy.Agent.SetDestination(player.transform.position);
        enemy.Animator.SetBool("IsWalking", false);
        enemy.Animator.SetTrigger("JumpAttack");
        enemy.Agent.speed += baseSpeed / 2;
    }

    public override void Exit()
    {
        enemy.Agent.speed -= baseSpeed / 2;
    }

    public override void Update()
    {
        
    }
}
