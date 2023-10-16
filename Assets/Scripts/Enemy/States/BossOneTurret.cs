using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Boss/OneTurret")]
public class BossOneTurret : State
{
    EnemyBoss enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyBoss)parent;
    }
    public override void ChangeState()
    {
        if (enemy.Turrets.Count <= 0)
        {
            enemy.StateMachine.SetState(typeof(BossComputerOnly));
        }
    }

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        foreach (Turret turret in enemy.Turrets)
        {
            turret.ClearToShoot();
        }
    }
}
