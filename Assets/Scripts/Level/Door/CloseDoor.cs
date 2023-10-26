using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    private Door door;
    public Player Player;
    public GameObject WaveSpawner;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Player.inWaveRoom = true;
            WaveSpawner.SetActive(true);
            door.CloseDoor();
            Destroy(this.gameObject);
        }
    }
}
