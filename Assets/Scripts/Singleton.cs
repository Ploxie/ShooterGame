using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    protected static T instance;

    public static T GetInstance()
    {
        if (instance == null)
            instance = new();

        return instance;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}