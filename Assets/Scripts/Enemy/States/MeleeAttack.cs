using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Attack")]
public class MeleeAttack : State
{
    EnemyMelee enemy;
    Player player;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyMelee)parent;
        player = enemy.Player;
    }
    public override void ChangeState()
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance <= enemy.AttackRange)
            {
                enemy.StateMachine.SetState(typeof(MeleeAttack));
            }
            else if (distance > enemy.AttackRange)
            {
                enemy.StateMachine.SetState(typeof(MeleeRunToPlayer));
            }

        }
    }

    public override void Enter()
    {
        enemy.Animator.SetBool("IsWalking", false);
        enemy.Agent.isStopped = true;
        if (Random.Range(1, 100) % 2 == 1)
            enemy.Animator.SetTrigger("Punch");
        else
            enemy.Animator.SetTrigger("Swipe");
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        var lookPos = player.transform.position - enemy.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 2);
    }
}
