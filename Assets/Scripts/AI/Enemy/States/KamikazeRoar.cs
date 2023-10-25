using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Roar")]

public class KamikazeRoar : State
{
    protected EnemyKamikaze enemyKamikaze;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyKamikaze = (EnemyKamikaze)parent;
    }
    public override void ChangeState()
    {
        if (enemyKamikaze.HasRoared)
        {
            enemyKamikaze.StateMachine.SetState(typeof(KamikazeRunToPlayer));
        }
    }

    public override void Enter()
    {
        enemyKamikaze.PlaySound("roarkamikaze");
        enemyKamikaze.Animator.SetTrigger("Roar");
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
