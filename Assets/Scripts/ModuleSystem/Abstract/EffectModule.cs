using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public abstract class EffectModule : Module
{
    public EffectStats BlueprintStats;

    public abstract StatusEffect GetStatusEffect();
}
