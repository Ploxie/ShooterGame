using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/RunToPlayer")]
public class KamikazeRunToPlayer : RunToPlayer
{
    protected EnemyKamikaze enemyKamikaze;

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

    }

    public override void Enter()
    {
        base.Enter();
        enemyKamikaze.Animator.SetBool("IsWalking", true);
    }
}
