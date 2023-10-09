using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Idle")]

public class Idle : State
{
    protected Enemy enemy;

    protected Vector3 targetLocation;

    protected Player player;

     [SerializeField] protected float detectionRange = 5;


    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        if (Vector3.Distance(enemy.transform.position, player.transform.position) < detectionRange)
        {
            enemy.StateMachine.SetState(typeof(RunToPlayer));
        }
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}
