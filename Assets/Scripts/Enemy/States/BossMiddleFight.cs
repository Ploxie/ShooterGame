using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Boss/MiddleFight")]
public class BossMiddleFight : State
{
    EnemyBoss enemy;

    private double spawnTimer = 5000;
    private double lastSpawn;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyBoss)parent;
        lastSpawn = Utils.GetUnixMillis();
    }
    public override void ChangeState()
    {
        if (enemy.Health <= enemy.MaxHealth / 2)
        {
            enemy.StateMachine.SetState(typeof(BossOneTurret));
        }
    }

    public override void Enter()
    {
        enemy.Shield.SetActive(false);
    }

    public override void Exit()
    {
        enemy.Shield.SetActive(true);
    }

    public override void Update()
    {
        if (Utils.GetUnixMillis() - lastSpawn < spawnTimer)
            return;
        lastSpawn = Utils.GetUnixMillis();
        foreach (SpawnEnemy spawn in enemy.SpawnPositions)
        {
            spawn.SpawnRandomEnemy();
        }
    }
}
