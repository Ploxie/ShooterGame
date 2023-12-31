using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySystem : MonoBehaviour
{

    [SerializeField] private List<Key.KeyType> keyList;

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
    }

    public void AddKey(Key.KeyType keyType)
    {
        Debug.Log($"Added key: {keyType}");
        keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        Debug.Log($"Removed key: {keyType}");
        keyList.Remove(keyType);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        if (keyType == Key.KeyType.None)
            return true;
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Key key = collider.GetComponent<Key>();
        if (key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        Door keyDoor = collider.GetComponent<Door>();
        if (keyDoor != null)
        {
            if (ContainsKey(keyDoor.GetKeyType()))
            {
                RemoveKey(keyDoor.GetKeyType());
                keyDoor.OpenDoor();
            }
            else if (keyDoor.GetKeyType() != Key.KeyType.Boss)
            {
                Key[] keys = FindObjectsOfType<Key>();
                Key actualKey = keys[0];
                foreach (Key potentialKey in keys)
                {
                    if (potentialKey.GetKeyType() == keyDoor.GetKeyType())
                        actualKey = potentialKey;
                }
                if (actualKey != null)
                {
                    EventManager.GetInstance().TriggerEvent(new KeyNeededEvent(actualKey, keyDoor));
                }
            }
        }
       
    }
}