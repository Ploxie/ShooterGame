using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu (menuName = "States/Enemy/RunToPlayer")]
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
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        targetLocation = player.transform.position;
        agent.SetDestination(targetLocation);
    }
}
