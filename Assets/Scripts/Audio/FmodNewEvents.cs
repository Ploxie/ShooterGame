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
