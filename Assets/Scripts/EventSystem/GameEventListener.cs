using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventResponse : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent GameEvent;
    public EventResponse EventResponse;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        EventResponse.Invoke(sender, data);
    }
}