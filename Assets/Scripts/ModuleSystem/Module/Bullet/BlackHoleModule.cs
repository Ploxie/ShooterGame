

public class BlackHoleModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        return new BlackHole();
    }
}