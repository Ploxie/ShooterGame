using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Dive")]
public class KamikazeDive : State
{
    EnemyKamikaze enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyKamikaze)parent;
    }
    public override void ChangeState()
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            enemy.StateMachine.SetState(typeof(KamikazeDeath));
        }
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {

    }

    public override void Enter()
    {
        enemy.Animator.SetTrigger("Dive");
    }
}
