using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Attack")]
public class MeleeAttack : State
{
    protected EnemyMelee enemyMelee;
    protected Assets.Scripts.Entity.Player player;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyMelee = (EnemyMelee)parent;
        player = enemyMelee.Player;
    }

    public override void ChangeState()
    {
        if (enemyMelee.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            float distance = Vector3.Distance(player.transform.position, enemyMelee.transform.position);
            if (distance <= enemyMelee.AttackRange)
            {
                enemyMelee.StateMachine.SetState(typeof(MeleeAttack));
            }
            else if (distance > enemyMelee.AttackRange)
            {
                enemyMelee.StateMachine.SetState(typeof(MeleeRunToPlayer));
            }

        }
    }

    public override void Enter()
    {
        enemyMelee.Animator.SetBool("IsWalking", false);
        enemyMelee.Agent.isStopped = true;
        if (Random.Range(1, 100) % 2 == 1)
            enemyMelee.Animator.SetTrigger("Punch");
        else
            enemyMelee.Animator.SetTrigger("Swipe");
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        var lookPos = player.transform.position - enemyMelee.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        enemyMelee.transform.rotation = Quaternion.Slerp(enemyMelee.transform.rotation, rotation, Time.deltaTime * 2);
    }
}
