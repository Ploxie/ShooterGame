

public class PiercingModule : BulletModule
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
        return new Piercing(GetData());
    }
}