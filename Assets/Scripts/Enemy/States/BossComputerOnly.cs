using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Enemy/Boss/ComputerOnly")]
public class BossComputerOnly : State
{
    EnemyBoss enemy;

    private double spawnTimer = 3000;
    private double lastSpawn;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyBoss)parent;
    }
    public override void ChangeState()
    {

    }

    public override void Enter()
    {
        enemy.Shield.SetActive(false);
    }

    public override void Exit()
    {

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
