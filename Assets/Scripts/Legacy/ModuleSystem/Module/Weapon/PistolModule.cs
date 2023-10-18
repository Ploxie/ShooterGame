using System.Collections.Generic;

public class PistolModule : WeaponModule
{
    public PistolModule()
    {
        TypeOfWeapon = WeaponType.Pistol;
    }

    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 800,
            LaunchSpeed = 1000,
            LaunchAngles = new List<float>() { 0 },
            AngleDeviation = 0
        };

        return data;
    }
}