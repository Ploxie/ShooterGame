using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Dive")]
public class KamikazeDive : State
{
    protected EnemyKamikaze enemyKamikaze;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyKamikaze = (EnemyKamikaze)parent;
    }

    public override void ChangeState()
    {
        if (enemyKamikaze.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            enemyKamikaze.StateMachine.SetState(typeof(KamikazeDeath));
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
        enemyKamikaze.Animator.SetTrigger("Dive");
    }
}
