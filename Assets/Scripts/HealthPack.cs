using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Entity;

public class HealthPack : PickupAble
{
    [SerializeField] private GameEvent OnHealthPackPickUpEvent;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] public float Healing;

    public override void Pickup()
    {
        pickUpText.SetActive(false);
        base.Pickup();
    }
}
