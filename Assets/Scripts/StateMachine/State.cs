using UnityEngine;

public abstract class State : ScriptableObject
{
    protected object runner;
    public virtual void Init(object parent) //Assign variables, get components
    {
        runner = parent;
    }

    public abstract void Enter(); //Execute things that happen on entry from a gameplay perspective

    public abstract void Update();

    public abstract void ChangeState();

    public abstract void Exit();
}
