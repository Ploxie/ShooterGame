using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    private Door door;
    public GameObject WaveSpawner;

    private void Start()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<Player>(out var player))
        {
            player.inWaveRoom = true;
            WaveSpawner.SetActive(true);
            //AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.doorSlam, door.transform.position);
            door.CloseDoor();
            Destroy(this.gameObject);
        }
    }
}
