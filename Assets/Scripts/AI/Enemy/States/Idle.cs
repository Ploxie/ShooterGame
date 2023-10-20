using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic/Idle")]

public class Idle : State
{
    protected Enemy enemy;

    protected Vector3 targetLocation;

    protected Assets.Scripts.Entity.Player player;

     [SerializeField] protected float detectionRange = 50;


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

    public override void Update()
    {
        
    }

    public override void ChangeState()
    {
        
    }
}
