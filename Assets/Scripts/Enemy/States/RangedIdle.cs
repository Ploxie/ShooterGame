using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/Idle")]
public class RangedIdle : Idle
{
    EnemyRanged enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyRanged)parent;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.Player.transform.position) < detectionRange)
        {
            enemy.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }
    public override void Enter()
    {
        base.Enter();
        enemy.Animator.SetBool("WalkForward", false);
        enemy.Animator.SetBool("WalkBackwards", false);
    }
    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}
