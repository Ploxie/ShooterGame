using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField] public List<GameEventListener> Listeners = new List<GameEventListener>();

    public void Raise(Component sender, object data)
    {
        for (int i = 0; i < Listeners.Count; i++)
        {
            Listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!Listeners.Contains(listener)) Listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (Listeners.Contains(listener)) Listeners.Remove(listener);
    }
}