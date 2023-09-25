using UnityEngine;

public class Piercing : BulletEffect
{
    public const int MAX_PIERCE_COUNT = 4;

    public int PierceCount;

    public Piercing()
    {
        PierceCount = 1;
    }

    public override void DoEffect(GameObject hitObject)
    {
        if (hitObject == null)
            return;

        if (PierceCount >= MAX_PIERCE_COUNT)
        {
            Parent.ShouldPierce = false;
            return;
        }

        Parent.ShouldPierce = true;
        PierceCount++;
    }
}