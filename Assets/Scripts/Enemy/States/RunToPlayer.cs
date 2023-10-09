using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Enemy/RunToPlayer")]
public /*abstract*/ class RunToPlayer : State
{
    Enemy enemy;

    Vector3 targetLocation;

    NavMeshAgent agent;

    Player player;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;
        agent = enemy.Agent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(player.transform.position, enemy.transform.position) > 10)
        {
            agent.isStopped = true;
            enemy.StateMachine.SetState(typeof(Idle));
        }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        agent.isStopped = false;
        targetLocation = player.transform.position;
        agent.SetDestination(targetLocation);
    }
}
