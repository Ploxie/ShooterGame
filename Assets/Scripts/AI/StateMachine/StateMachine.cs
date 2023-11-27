using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StateMachine
{
    [SerializeField] private List<State> states;

    private readonly Dictionary<Type, State> stateByType = new();
    [SerializeField] private State activeState;

    private object parent;

    public void Init(object parent)
    {
        this.parent = parent;

        foreach (var state in states)
        {
            State tempState = UnityEngine.Object.Instantiate(state);
            stateByType.Add(tempState.GetType(), tempState);
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
        activeState.Enter();
    }

    public State GetState()
    {
        return activeState;
    }

    public void Update()
    {
        activeState.Update();
        activeState.ChangeState();
    }
}
