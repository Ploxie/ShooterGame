using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private KeyType keyType;

    public enum KeyType
    {
        Red,
        Green,
        Blue,
        Boss,
        None
    }

    public void SetKeyType(KeyType type)
    {
        keyType = type;
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }
}
