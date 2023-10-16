using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Ranged/RunToPlayer")]
public class RangedRunToPlayer : RunToPlayer
{
    EnemyRanged enemy;

    public override void Init(object other)
    {
        base.Init(other);
        enemy = (EnemyRanged)other;
    }
    public override void ChangeState()
    {
        if (enemy.HasLineOfSightToPlayer())
        {
            float distance = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
            if (distance <= 15)
            {
                enemy.StateMachine.SetState(typeof(RangedShooting));
            }
        }
    }
    public override void Enter()
    {
        base.Enter();
        enemy.Animator.SetBool("WalkForward", true);
    }
    public override void Exit()
    {
        enemy.Animator.SetBool("WalkForward", false);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.HasLineOfSightToPlayer())
        {
            enemy.Shoot();
        }
    }

}
