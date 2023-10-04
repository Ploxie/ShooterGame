using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module
{
    public int Durability { get { return remainingUses; } }

    protected int remainingUses;
    
    public virtual void DecrementDurability(int amount)
    {
        remainingUses--;
    }

    public virtual void OnDurabilityDecremented() { }
}
