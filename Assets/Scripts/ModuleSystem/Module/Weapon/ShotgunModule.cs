using System.Collections.Generic;

public class ShotgunModule : WeaponModule
{
    public const int DURABILITY = 4000;

    public ShotgunModule()
    {
        this.remainingUses = DURABILITY;
        TypeOfWeapon = WeaponType.Shotgun;
    }

    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 1300,
            LaunchSpeed = 1000,
            LaunchAngles = new List<float>() { 0, 22.5f, -22.5f, 11.25f, -11.25f },
            AngleDeviation = 0
        };

        return data;
    }
}