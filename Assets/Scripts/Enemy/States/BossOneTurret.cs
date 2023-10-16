using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Boss/OneTurret")]
public class BossOneTurret : State
{
    protected EnemyBoss enemyBoss;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemyBoss = (EnemyBoss)parent;
    }
    public override void ChangeState()
    {
        if (enemyBoss.Turrets.Count <= 0)
        {
            enemyBoss.StateMachine.SetState(typeof(BossComputerOnly));
        }
    }

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        foreach (Turret turret in enemyBoss.Turrets)
        {
            turret.ClearToShoot();
        }
    }
}
