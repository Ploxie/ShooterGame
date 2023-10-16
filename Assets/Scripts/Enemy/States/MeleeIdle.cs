
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Idle")]
public class MeleeIdle : Idle
{
    EnemyMelee enemy;

    Player player;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyMelee)parent;
        player = enemy.Player;
    }
    public override void ChangeState()
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

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
    public override void Enter()
    {
        base.Enter();
        enemy.Animator.SetBool("IsWalking", false);
    }
}
