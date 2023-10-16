using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Ranged/Death")]
public class RangedDeath : Death
{
    EnemyRanged enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyRanged)parent;

    }
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
    }
}
