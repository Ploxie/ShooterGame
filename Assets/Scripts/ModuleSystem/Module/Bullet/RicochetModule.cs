

public class RicochetModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        return new Ricochet();
    }
}