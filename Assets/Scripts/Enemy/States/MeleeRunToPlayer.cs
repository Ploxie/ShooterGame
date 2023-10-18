
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/RunToPlayer")]

public class MeleeRunToPlayer : RunToPlayer
{
    protected EnemyMelee enemyMelee;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyMelee = (EnemyMelee)parent;
    }

    public override void ChangeState()
    {
        float distance = Vector3.Distance(player.transform.position, enemyMelee.transform.position);
        if (distance <= enemyMelee.AttackRange)
        {
            enemyMelee.StateMachine.SetState(typeof(MeleeAttack));
        }
        else if (distance < enemyMelee.JumpRange)
        {
            enemyMelee.StateMachine.SetState(typeof(MeleeJumpAttack));
        }
        else if (distance > enemyMelee.AttackRange)
        {
            enemyMelee.StateMachine.SetState(typeof(MeleeRunToPlayer));
        }
    }

    public override void Exit()
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        enemyMelee.Animator.SetBool("IsWalking", true);
    }
}
