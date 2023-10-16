using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgePickup : PickupAble
{
    [field: SerializeField] public Module Module { get; set; }
}
