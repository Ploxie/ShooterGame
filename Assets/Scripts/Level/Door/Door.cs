using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Key.KeyType keyType;


    private void Start()
    {

        var renderers = GetComponentsInChildren<Renderer>();

        if (renderers == null)
            return;

        foreach (var renderer in renderers)
        {
            if (renderer == null)
                continue;

            if (renderer.name == "Frame")
                continue;

            switch (keyType)
            {
                case Key.KeyType.Red:
                    renderer.material.color = Color.red;
                    break;
                case Key.KeyType.Blue:
                    renderer.material.color = Color.blue;
                    break;
                case Key.KeyType.Green:
                    renderer.material.color = Color.green;
                    break;
            }
        }

    }

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