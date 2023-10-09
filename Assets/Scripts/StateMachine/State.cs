using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    protected object runner;

    public virtual void Init(object parent)
    {
        runner = parent;
    }

    public abstract void Update();

    public abstract void ChangeState();

    public abstract void Exit();
}
