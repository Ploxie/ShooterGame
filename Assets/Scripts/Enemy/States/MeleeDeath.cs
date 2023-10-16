using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Melee/Death")]
public class MeleeDeath : Death
{
    EnemyMelee enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyMelee)parent;

    }
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
    }
}
