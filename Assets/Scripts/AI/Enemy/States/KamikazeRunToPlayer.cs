using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/RunToPlayer")]
public class KamikazeRunToPlayer : RunToPlayer
{
    protected EnemyKamikaze enemyKamikaze;
    EventInstance PlayaRun;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyKamikaze = (EnemyKamikaze)parent;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(player.transform.position, enemyKamikaze.transform.position) < enemyKamikaze.DiveRange)
        {
            enemyKamikaze.StateMachine.SetState(typeof(KamikazeDive));
        }
    }

    public override void Exit()
    {
        PlayaRun.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public override void Enter()
    {
        base.Enter();
        //PlayaRun = AudioFmodManager.instance.CreateFootstepInst(FmodEvents.instance.RunEnemyKamikaze);
        //PlayaRun.start();
        enemyKamikaze.Animator.SetBool("IsWalking", true);
    }
}
