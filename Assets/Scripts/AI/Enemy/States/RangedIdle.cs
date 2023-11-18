using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/Idle")]
public class RangedIdle : Idle
{
    protected EnemyRanged enemyRanged;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemyRanged = (EnemyRanged)parent;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(enemyRanged.transform.position, enemyRanged.Player.transform.position) < detectionRange)
        {
            enemyRanged.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }
    public override void Enter()
    {
        base.Enter();
        enemyRanged.Animator.SetBool("WalkForward", false);
        enemyRanged.Animator.SetBool("WalkBackwards", false);
    }
    //public override void Exit(){}

    public override void Update()
    {
        
    }
}
