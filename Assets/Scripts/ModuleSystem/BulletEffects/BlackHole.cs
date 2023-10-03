using System.Collections.Generic;
using UnityEngine;

public class BlackHole : BulletEffect
{
    public const float Range = 10;
    public const float RangeIncrements = 2;
    public const float PullStrength = 100;
    public const float PullStrengthIncrements = 50;

    public BlackHole(BulletEffectData data) : base(data) { }

    public override void DoEffect(GameObject _)
    {
        List<Living> inRange = Parent.EnemyManager.GetEnemiesInRange(Parent.transform.position, Range + RangeIncrements * (effectData.Strength - 1));
        foreach (Living enemy in inRange)
        {
            Vector3 direction = Parent.transform.position - enemy.transform.position;
            if (direction.magnitude == 0) //Prevent division with 0 in Normalize.
                continue;

            direction.Normalize();
            enemy.GetComponent<Rigidbody>().AddRelativeForce(direction * (PullStrength + PullStrengthIncrements * (effectData.Strength - 1)));
        }
    }
}