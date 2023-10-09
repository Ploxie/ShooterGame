using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StateMachine
{
    [SerializeField] private List<State> states;

    private readonly Dictionary<Type, State> stateByType = new();
    private State activeState;
    private object parent;

    public void Init(object parent)
    {
        this.parent = parent;
        //states.ForEach(s => stateByType.Add(s.GetType(), s));
        foreach (State s in states)
        {
            stateByType.Add(s.GetType(), s);
        }

        SetState(states[0].GetType());
    }

    public void SetState(Type newStateType)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = stateByType[newStateType];
        activeState.Init(parent);
    }
    public void Update()
    {
        activeState.Update();
        activeState.ChangeState();
    }
}
