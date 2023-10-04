

public class ExplosiveModule : BulletModule
{
    public const int DURABILITY = 200;

    public ExplosiveModule()
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
        return new Explosive(GetData());
    }
}