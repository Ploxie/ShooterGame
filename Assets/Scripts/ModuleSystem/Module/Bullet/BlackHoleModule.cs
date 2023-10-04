

public class BlackHoleModule : BulletModule
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
        return new BlackHole(GetData());
    }
}