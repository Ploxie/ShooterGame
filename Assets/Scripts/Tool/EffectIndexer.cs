using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectIndexer : ModuleIndexer
{
    public SimulationHerald Herald;
    public StatusID ModuleID;
    public Toggle AssignedToggle;

    public override void ToggleChanged(bool value)
    {
        Herald.ToggleModule(ModuleType.Weapon, (int)ModuleID);
    }
}
