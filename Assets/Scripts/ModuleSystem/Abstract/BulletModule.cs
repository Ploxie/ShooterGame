using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletModule : Module
{
    public abstract BulletEffectData GetData();
    public abstract BulletEffect GetBulletEffect();
}
