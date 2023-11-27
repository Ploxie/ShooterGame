using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodEvents : MonoBehaviour
{
    [field: Header("Slam")]
    [field: SerializeField] public EventReference slamsGround { get; private set; }

    [field: Header("Death kamikaze")]
    [field: SerializeField] public EventReference BoomAndDeathKamikaze { get; private set; }

    [field: Header("ShootSound")]
    [field: SerializeField] public EventReference ShootSound { get; private set; }

    [field: Header("Death Ranged")]
    [field: SerializeField] public EventReference deathRanged { get; private set; }

    [field: Header("Death Melee")]
    [field: SerializeField] public EventReference deathMelee { get; private set; }

    [field: Header("Death kamikaze")]
    [field: SerializeField] public EventReference roarKamikaze { get; private set; }

    [field: Header("Scream melee")]
    [field: SerializeField] public EventReference ScreamMelee { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambienceTest { get; private set; }

    [field: Header("GameLoopMusicBack")]
    [field: SerializeField] public EventReference MusicLoop { get; private set; }

    [field: Header("ToggleProj")]
    [field: SerializeField] public EventReference ToggleProj { get; private set; }

    [field: Header("PickUpProj")]
    [field: SerializeField] public EventReference PickUpProj { get; private set; }

    [field: Header("ToggleStatus")]
    [field: SerializeField] public EventReference ToggleStatus { get; private set; }

    [field: Header("PickUpStatus")]
    [field: SerializeField] public EventReference PickUpStatus { get; private set; }

    [field: Header("ToggleWeapon")]
    [field: SerializeField] public EventReference ToggleWeapon { get; private set; }

    [field: Header("PickUpWeapon")]
    [field: SerializeField] public EventReference PickUpWeapon { get; private set; }

    [field: Header("Sniper")]
    [field: SerializeField] public EventReference Sniper { get; private set; }

    [field: Header("Pistol")]
    [field: SerializeField] public EventReference Pistol { get; private set; }

    [field: Header("SMG")]
    [field: SerializeField] public EventReference SMG { get; private set; }

    [field: Header("Shotgun")]
    [field: SerializeField] public EventReference Shotgun { get; private set; }

    [field: Header("Assualt")]
    [field: SerializeField] public EventReference Assualt { get; private set; }
    [SerializeField] public List<EventReference> GunSound = new List<EventReference>();
    // Start is called before the first frame update
    public static FmodEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More then one audioManager");
        }
        instance = this;
    }
}
