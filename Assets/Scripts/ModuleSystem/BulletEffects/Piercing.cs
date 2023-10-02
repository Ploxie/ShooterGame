using UnityEngine;

public class Piercing : BulletEffect
{
    public const int MAX_PIERCE_COUNT = 4;
    public const int PIERCE_COUNT_INCREMENT = 1;

    public int PierceCount;

    public Piercing(BulletEffectData data) : base(data) { PierceCount = 1; }

    public override void DoEffect(GameObject hitObject)
    {
        if (hitObject == null)
            return;

        if (PierceCount >= MAX_PIERCE_COUNT + PIERCE_COUNT_INCREMENT * (effectData.Strength - 1))
        {
            Parent.ShouldPierce = false;
            return;
        }

        Parent.ShouldPierce = true;
        PierceCount++;
    }
}