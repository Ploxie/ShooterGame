using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Death")]
public class MeleeDeath : Death
{   
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
    }
}
