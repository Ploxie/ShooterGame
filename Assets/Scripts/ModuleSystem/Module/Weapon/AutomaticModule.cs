using System.Collections.Generic;

public class AutomaticModule : WeaponModule
{
    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 100,
            LaunchSpeed = 1000,
            LaunchAngles = new List<float>() { 0 },
            AngleDeviation = 2
        };

        return data;
    }
}