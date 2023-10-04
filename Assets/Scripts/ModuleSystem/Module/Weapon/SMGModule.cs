using System.Collections.Generic;

public class SMGModule : WeaponModule
{
    public SMGModule()
    {
        TypeOfWeapon = WeaponType.SMG;
    }

    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 70,
            LaunchSpeed = 1000,
            LaunchAngles = new List<float>() { 0 },
            AngleDeviation = 20
        };

        return data;
    }
}