using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/Shooting")]
public class RangedShooting : State
{
    protected EnemyRanged enemyRanged;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemyRanged = (EnemyRanged)parent;
    }
    public override void ChangeState()
    {
        float distance = Vector3.Distance(enemyRanged.Player.transform.position, enemyRanged.transform.position);
        if (distance <= 5)
        {
            enemyRanged.StateMachine.SetState(typeof(RangedRunFromPlayer));
        }
        else if (distance > 20)
        {
            enemyRanged.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }

    public override void Enter()
    {
        enemyRanged.Agent.isStopped = true;
        enemyRanged.Animator.SetBool("IdleState", true);
    }

    public override void Exit()
    {
        enemyRanged.Agent.isStopped = false;
        enemyRanged.Animator.SetBool("IdleState", false);
    }

    public override void Update()
    {
        if (enemyRanged.HasLineOfSightToPlayer())
        {
            var lookPos = enemyRanged.Player.transform.position - enemyRanged.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            enemyRanged.transform.rotation = Quaternion.Slerp(enemyRanged.transform.rotation, rotation, Time.deltaTime * 2);
            enemyRanged.Shoot();
        }
    }
}
