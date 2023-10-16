using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/RunFromPlayer")]
public class RangedRunFromPlayer : State
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
        if (distance > 20)
        {
            enemy.StateMachine.SetState(typeof(RangedRunToPlayer));
        }
    }

    public override void Enter()
    {
        enemy.Agent.isStopped = false;
        enemy.Agent.updateRotation = false;
        enemy.Animator.SetBool("WalkBackwards", true);
    }

    public override void Exit()
    {
        enemy.Agent.updateRotation = true;
        enemy.Animator.SetBool("WalkBackwards", false);
    }

    public override void Update()
    {
        enemy.Agent.SetDestination(enemy.transform.position + (enemy.transform.position - enemy.Player.transform.position));
        if (enemy.HasLineOfSightToPlayer())
        {
            
            Quaternion rotation = Quaternion.LookRotation(enemy.Player.transform.position - enemy.transform.position);
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rotation, 60f * Time.deltaTime);
            enemy.Shoot();

        }
    }
}
