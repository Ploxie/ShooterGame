using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Enemy/RunToPlayer")]
public /*abstract*/ class RunToPlayer : State
{
    protected Enemy enemy;

    protected Vector3 targetLocation;

    protected NavMeshAgent agent;

    protected Player player;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;
        agent = enemy.Agent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(player.transform.position, enemy.transform.position) > 20)
        {
            agent.isStopped = true;
            enemy.StateMachine.SetState(typeof(Idle));
        }
        else if (Vector3.Distance(player.transform.position, enemy.transform.position) < 2)
        {
            agent.isStopped = true;
            enemy.StateMachine.SetState(typeof(Death));
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
