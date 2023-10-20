using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorTrigger : MonoBehaviour
{
    Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        door.CloseDoor();
    }
}
