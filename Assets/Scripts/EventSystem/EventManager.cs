using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    private static EventManager instance;

    private void Awake()
    {
        instance = this;
    }
    public static EventManager GetInstance()
    {
        return instance;
    }
    private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
    {
        if (delegateLookup.ContainsKey(del))
            return null;

        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }

    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        AddDelegate<T>(del);
        Debug.Log("Added listener " + typeof(T));
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    public void TriggerEvent(GameEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
        else
        {
            //Debug.Log("Event: " + e.GetType() + " has no listeners");
        }
    }
}
