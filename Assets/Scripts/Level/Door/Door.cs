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
        AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.doorSlam, this.transform.position);
        anim.SetTrigger("DoorOpen");
    }
    public void CloseDoor()
    {
        Debug.Log("Close Door");
        AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.doorOpen, this.transform.position);
        anim.SetTrigger("Reset");
    }
}