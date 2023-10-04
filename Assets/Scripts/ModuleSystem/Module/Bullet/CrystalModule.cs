

public class CrystalModule : BulletModule
{
    public const int DURABILITY = 1000;

    public CrystalModule()
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
        Crystal effect = new Crystal(GetData());
        effect.ActivationDelay = 500;
        return effect;
    }
}