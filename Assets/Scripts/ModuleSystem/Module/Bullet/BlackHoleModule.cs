

public class BlackHoleModule : BulletModule
{
    public const int DURABILITY = 500;

    public BlackHoleModule()
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
        return new BlackHole(GetData());
    }
}