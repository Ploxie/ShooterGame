using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/RunToPlayer")]
public class KamikazeRunToPlayer : RunToPlayer
{
    EnemyKamikaze enemy;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyKamikaze)parent;
    }
    public override void ChangeState()
    {
        base.ChangeState();
        if (Vector3.Distance(player.transform.position, enemy.transform.position) < 3)
        {
            enemy.StateMachine.SetState(typeof(KamikazeDive));
        }
    }

    public override void Exit()
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemy.Animator.SetBool("IsRunning", true);
    }
}
