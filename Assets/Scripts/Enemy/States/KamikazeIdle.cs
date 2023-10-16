
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Idle")]
public class KamikazeIdle : Idle
{
    EnemyKamikaze enemy;

    Player player;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyKamikaze)parent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance < detectionRange)
        {
            enemy.StateMachine.SetState(typeof(KamikazeRunToPlayer));
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
