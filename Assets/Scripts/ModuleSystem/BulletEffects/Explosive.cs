using System.Collections.Generic;
using UnityEngine;

public class Explosive : BulletEffect
{
    public override void DoEffect(GameObject _)
    {
        List<Living> inRange = Parent.EnemyManager.GetEnemiesInRange(Parent.transform.position, 10);
        foreach (Living living in inRange)
        {
            living.TakeDamage(Parent.Damage * 10);
        }
    }
}