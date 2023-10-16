using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Ranged/Shooting")]
public class RangedShooting : State
{
    EnemyRanged enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyRanged)parent;
    }
    public override void ChangeState()
    {
        float distance = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position);
        if (distance <= 5)
        {
            enemy.StateMachine.SetState(typeof(RangedRunFromPlayer));
        }
        else if (distance > 20)
        {
            enemy.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetBool("IdleState", true);
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
        enemy.Animator.SetBool("IdleState", false);
    }

    public override void Update()
    {
        if (enemy.HasLineOfSightToPlayer())
        {
            var lookPos = enemy.Player.transform.position - enemy.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 2);
            enemy.Shoot();
        }
    }
}
