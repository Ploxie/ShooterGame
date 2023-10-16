
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/RunToPlayer")]

public class MeleeRunToPlayer : RunToPlayer
{
    EnemyMelee enemy;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyMelee)parent;
    }

    public override void ChangeState()
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance <= enemy.AttackRange)
        {
            enemy.StateMachine.SetState(typeof(MeleeAttack));
        }
        else if (distance < enemy.JumpRange)
        {
            enemy.StateMachine.SetState(typeof(MeleeJumpAttack));
        }
        else if (distance > enemy.AttackRange)
        {
            enemy.StateMachine.SetState(typeof(MeleeRunToPlayer));
        }
    }

    public override void Exit()
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        enemy.Animator.SetBool("IsWalking", true);
    }
}
