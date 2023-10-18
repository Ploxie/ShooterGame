using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public abstract class EffectModule : Module2
{
    public const int INSTANT = -1;

    public abstract StatusEffectData GetData();
    public abstract StatusEffect GetStatusEffect();
}
