
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Idle")]
public class MeleeIdle : Idle
{
    protected EnemyMelee enemyMelee;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyMelee = (EnemyMelee)parent;
        
    }

    public override void ChangeState()
    {
        player = enemyMelee.Player;

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

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
    public override void Enter()
    {
        base.Enter();
        enemyMelee.Animator.SetBool("IsWalking", false);
    }
}
