using Assets.Scripts.Entity;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Enemy/Generic/RunToPlayer")]
public abstract class RunToPlayer : State
{
    protected Enemy enemy;

    protected Vector3 targetLocation;

    protected NavMeshAgent agent;

    protected Assets.Scripts.Entity.Player player;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(player.transform.position, enemy.transform.position) > 20)
        {
            enemy.Agent.isStopped = true;
            enemy.StateMachine.SetState(typeof(Idle));
        }
    }

    public override void Update()
    {       
        enemy.Agent.SetDestination(player.transform.position);
    }

    public override void Enter()
    {
        enemy.Agent.isStopped = false;
    }
}
