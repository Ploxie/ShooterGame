
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Idle")]
public class KamikazeIdle : Idle
{
    protected EnemyKamikaze enemyKamikaze;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyKamikaze = (EnemyKamikaze)parent;
        
    }

    public override void ChangeState()
    {
        player = enemyKamikaze.Player;

        float distance = Vector3.Distance(player.transform.position, enemyKamikaze.transform.position);
        if (distance < detectionRange)
        {
            enemyKamikaze.StateMachine.SetState(typeof(KamikazeRunToPlayer));
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
        enemyKamikaze.Animator.SetBool("IsWalking", false);
    }
}
