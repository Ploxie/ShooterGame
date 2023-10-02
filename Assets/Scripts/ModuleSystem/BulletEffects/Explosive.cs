using System.Collections.Generic;
using UnityEngine;

public class Explosive : BulletEffect
{
    public const float Range = 10;
    public const float RangeIncrement = 2;

    public const float Damage = 10;

    public Explosive(BulletEffectData data) : base(data) { }

    public override void DoEffect(GameObject _)
    {
        List<Living> inRange = Parent.EnemyManager.GetEnemiesInRange(Parent.transform.position, Range + RangeIncrement * (effectData.Strength - 1));
        foreach (Living living in inRange)
        {
            living.TakeDamage(Parent.Damage + Damage * effectData.Strength);
        }
    }
}