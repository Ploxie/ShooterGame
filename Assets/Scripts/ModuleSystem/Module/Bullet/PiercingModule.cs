

public class PiercingModule : BulletModule
{
    public const int DURABILITY = 1500;

    public PiercingModule()
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
        return new Piercing(GetData());
    }
}