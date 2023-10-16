
using UnityEngine;


[CreateAssetMenu(menuName = "States/Enemy/Generic/RunToPlayer")]
public abstract class RunToPlayer : State
{
    Enemy enemy;
    protected Player player;
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
