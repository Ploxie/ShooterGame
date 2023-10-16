using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic/Idle")]

public abstract class Idle : State
{
    Enemy enemy;
    protected float detectionRange = 50;


    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;
    }
    public override void Enter()
    {
       enemy.Agent.isStopped = true;
    }
    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }
}
