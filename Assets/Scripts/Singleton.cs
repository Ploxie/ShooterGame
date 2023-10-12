using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => instance;

    protected static T instance;

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.LogError($"Failed to create an instance of {typeof(T)}");
        }
    }
}