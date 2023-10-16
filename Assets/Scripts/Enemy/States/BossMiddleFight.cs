using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Boss/MiddleFight")]
public class BossMiddleFight : State
{
    protected EnemyBoss enemyBoss;

    private double spawnTimer = 5000;
    private double lastSpawn;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyBoss = (EnemyBoss)parent;
        lastSpawn = Utils.GetUnixMillis();
    }

    public override void ChangeState()
    {
        if (enemyBoss.Health.CurrentHealth <= enemyBoss.Health.MaxHealth / 2)
        {
            enemyBoss.StateMachine.SetState(typeof(BossOneTurret));
        }
    }

    public override void Enter()
    {
        enemyBoss.Shield.SetActive(false);
    }

    public override void Exit()
    {
        enemyBoss.Shield.SetActive(true);
    }

    public override void Update()
    {
        if (Utils.GetUnixMillis() - lastSpawn < spawnTimer)
            return;

        lastSpawn = Utils.GetUnixMillis();
        foreach (EnemySpawner spawn in enemyBoss.SpawnPositions)
        {
            spawn.SpawnRandomEnemy();
        }
    }
}
