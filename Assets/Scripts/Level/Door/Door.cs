using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Key.KeyType keyType;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetKeyType(Key.KeyType type)
    {
        keyType = type;
    }

    public Key.KeyType GetKeyType()
    {
        return keyType;
    }

    public void OpenDoor()
    {
        Debug.Log("Open Door");
        anim.SetTrigger("DoorOpen");
    }
    public void CloseDoor()
    {
        Debug.Log("Close Door");
        anim.SetTrigger("Reset");
    }
}