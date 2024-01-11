using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIndexer : ModuleIndexer
{
    public SimulationHerald Herald;
    public WeaponID ModuleID;
    public Toggle AssignedToggle;

    public override void ToggleChanged(bool value)
    {
        Herald.ToggleModule(ModuleType.Weapon, (int)ModuleID);
    }
}
