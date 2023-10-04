

public class RicochetModule : BulletModule
{
    public const int DURABILITY = 1500;

    public RicochetModule()
    {
        this.remainingUses = DURABILITY;
    }

    public override BulletEffectData GetData()
    {
        return new BulletEffectData
        {
            Strength = 1
        };
    }

    public override BulletEffect GetBulletEffect()
    {
        return new Ricochet(GetData());
    }
}