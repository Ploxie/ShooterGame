

public class PiercingModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        return new Piercing();
    }
}