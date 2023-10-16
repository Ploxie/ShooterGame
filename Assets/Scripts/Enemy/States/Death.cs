
using Assets.Scripts.Entity;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic/Death")]
public abstract class Death : State
{
    protected Enemy enemy;
    protected double deathTimerStarted;
    protected double deathTimerDuration = 1000;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;

    }
    public override void ChangeState()
    {
        
    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {

        if (Utils.GetUnixMillis() - deathTimerStarted >= deathTimerDuration)
        {
            Destroy(enemy.gameObject);
        }
    }

    public override void Enter()
    {
        deathTimerStarted = Utils.GetUnixMillis();
        enemy.Agent.isStopped = true;
        enemy.GetComponent<Collider>().enabled = false;
    }
}
