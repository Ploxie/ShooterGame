using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/RunToPlayer")]
public class RangedRunToPlayer : RunToPlayer
{
    protected EnemyRanged enemyRanged;

    public override void Init(object other)
    {
        base.Init(other);
        enemyRanged = (EnemyRanged)other;
    }

    public override void ChangeState()
    {
        if (enemyRanged.HasLineOfSightToPlayer())
        {
            float distance = Vector3.Distance(enemyRanged.Player.transform.position, enemyRanged.transform.position);
            if (distance <= 15)
            {
                enemyRanged.StateMachine.SetState(typeof(RangedShooting));
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        enemyRanged.Animator.SetBool("WalkForward", true);
    }

    public override void Exit()
    {
        enemyRanged.Animator.SetBool("WalkForward", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemyRanged.HasLineOfSightToPlayer())
        {
            enemyRanged.Shoot();
        }
    }

}
