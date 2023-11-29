using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/RunFromPlayer")]
public class RangedRunFromPlayer : State
{
    protected EnemyRanged enemyRanged;
    EventInstance PlayaRun;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemyRanged = (EnemyRanged)parent;

    }

    public override void ChangeState()
    {
        float distance = Vector3.Distance(enemyRanged.Player.transform.position, enemyRanged.transform.position);
        if (distance > 20)
        {
            enemyRanged.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }

    public override void Enter()
    {
        enemyRanged.Agent.isStopped = false;
        enemyRanged.Agent.updateRotation = false;
        PlayaRun = AudioFmodManager.instance.CreateFootstepInst(FmodEvents.instance.RunEnemyKamikaze);
        PlayaRun.start();
        enemyRanged.Animator.SetBool("WalkBackwards", true);
    }

    public override void Exit()
    {
        enemyRanged.Agent.updateRotation = true;
        PlayaRun.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        enemyRanged.Animator.SetBool("WalkBackwards", false);
    }

    public override void Update()
    {
        enemyRanged.Agent.SetDestination(enemyRanged.transform.position + (enemyRanged.transform.position - enemyRanged.Player.transform.position));
        if (enemyRanged.HasLineOfSightToPlayer())
        {            
            Quaternion rotation = Quaternion.LookRotation(enemyRanged.Player.transform.position - enemyRanged.transform.position);
            enemyRanged.transform.rotation = Quaternion.RotateTowards(enemyRanged.transform.rotation, rotation, 60f * Time.deltaTime);
            enemyRanged.Shoot();

        }
    }
}
