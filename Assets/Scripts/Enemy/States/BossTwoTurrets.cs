using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Boss/TwoTurrets")]
public class BossTwoTurrets : State
{
    EnemyBoss enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyBoss)parent;
    }
    public override void ChangeState()
    {
        if (enemy.Turrets.Count <= enemy.InitialNumberOfTurrets / 2)
        {
            enemy.StateMachine.SetState(typeof(BossMiddleFight));
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
