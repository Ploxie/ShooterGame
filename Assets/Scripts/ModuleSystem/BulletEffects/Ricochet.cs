using UnityEngine;

public class Ricochet : BulletEffect
{
    public const int MAX_RICOCHET_COUNT = 4;
    public const int RICOCHET_COUNT_INCREMENT = 1;

    public int RicochetCount;

    public Ricochet(BulletEffectData data) : base(data) { }

    public override void DoEffect(GameObject hitObject)
    {
        if (hitObject == null || hitObject.tag != "Wall")
            return;

        if (RicochetCount >= MAX_RICOCHET_COUNT + RICOCHET_COUNT_INCREMENT * (effectData.Strength - 1))
        {
            Parent.ShouldPierce = false;
            return;
        }

        Parent.ShouldPierce = true;
        Rigidbody bulletRigidbody = Parent.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = Vector3.Reflect(bulletRigidbody.velocity, hitObject.transform.rotation * Vector3.forward);
        RicochetCount++;

    }
}