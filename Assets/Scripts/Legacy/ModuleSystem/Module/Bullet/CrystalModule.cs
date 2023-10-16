

public class CrystalModule : BulletModule
{
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