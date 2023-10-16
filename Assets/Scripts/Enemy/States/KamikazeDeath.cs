using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Death")]

public class KamikazeDeath : Death
{
    EnemyKamikaze enemy;
    float explosionSize = 0;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (EnemyKamikaze)parent;

    }
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Explode();
    }
    public override void Update()
    {
        explosionSize += Time.deltaTime / 10;
        enemy.ExplosionDamageHitBox.transform.localScale += new Vector3(explosionSize, explosionSize, explosionSize);
        base.Update();
    }
}
