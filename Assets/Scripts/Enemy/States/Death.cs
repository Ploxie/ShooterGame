using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Death")]
public class Death : State
{
    TestEnemy enemy;
    protected double deathTimerStarted;
    protected double deathTimerDuration = 1000;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (TestEnemy)parent;
        deathTimerStarted = Utils.GetUnixMillis();
        enemy.Agent.isStopped = true;
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
}
