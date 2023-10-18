using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/Death")]
public class RangedDeath : Death
{ 
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
    }
}
