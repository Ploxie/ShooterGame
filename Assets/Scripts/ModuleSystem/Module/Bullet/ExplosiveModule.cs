

public class ExplosiveModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        return new Explosive();
    }
}