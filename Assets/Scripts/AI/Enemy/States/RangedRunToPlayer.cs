using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/RunToPlayer")]
public class RangedRunToPlayer : RunToPlayer
{
    protected EnemyRanged enemyRanged;
    EventInstance PlayaRun;

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
        PlayaRun = AudioFmodManager.instance.CreateFootstepInst(FmodEvents.instance.RunEnemyKamikaze);
        PlayaRun.start();
        enemyRanged.Animator.SetBool("WalkForward", true);
    }

    public override void Exit()
    {
        PlayaRun.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
