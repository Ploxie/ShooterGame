using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Boss/TwoTurrets")]
public class BossTwoTurrets : State
{
    protected EnemyBoss enemyBoss;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyBoss = (EnemyBoss)parent;
    }

    public override void ChangeState()
    {
        if (enemyBoss.Turrets.Count <= enemyBoss.InitialNumberOfTurrets / 2)
        {
            enemyBoss.StateMachine.SetState(typeof(BossMiddleFight));
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
