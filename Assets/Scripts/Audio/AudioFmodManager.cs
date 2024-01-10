using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
//using UnityEditor.ShaderGraph.Internal;
using Assets.Scripts.Entity;
using FMOD;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class AudioFmodManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]

    public float masterVolume = 1f;
    [Range(0, 1)]
    public float Effects = 1f;
    [Range(0, 1)]
    public float GameLoops = 1f;
    [Range(0, 1)]
    public float Music = 1f;
    [Range(0, 1)]
    public float Ambient = 1f;
    [Range(0, 1)]

    private Bus MasterBus;
    private Bus GameLoopBus;
    private Bus EffectBus;
    private Bus MusicBus;
    private Bus AmbientBus;


    private List<EventInstance> eventInstanses;

    public EventInstance eventInstanceAmb, eventInstanceMusic;
    private Camera cam;

    private EventInstance MusicInstance;
    private float area = 0;

    public static AudioFmodManager instance { get; private set; }
    private float EnemiesInCombat = 0f, dealingWithenemies = 0f;

    private void Awake()
    {
        if(instance != null)
        {
            UnityEngine.Debug.Log("More then one audioManager");
        }
        instance = this;

        eventInstanses = new List<EventInstance>();

        MasterBus = RuntimeManager.GetBus("bus:/");
        GameLoopBus = RuntimeManager.GetBus("bus:/GameLoop");
        EffectBus = RuntimeManager.GetBus("bus:/Effects");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        AmbientBus = RuntimeManager.GetBus("bus:/Ambience");
    }
    private void Start()
    {
        EventManager.GetInstance().AddListener<EnemyEnterCombatEvent>(OnEnemyEnterCombat);
        EventManager.GetInstance().AddListener<EnemyLeaveCombatEvent>(OnEnemyLeaveCombat);
    }
    private void Update()
    {
        MasterBus.setVolume(masterVolume);
        AmbientBus.setVolume(Ambient);
        MusicBus.setVolume(Music);
        EffectBus.setVolume(Effects);
        GameLoopBus.setVolume(GameLoops);
    }
    //public void SetMasterVolume(float value)
    //{
    //    masterVolume = value;
    //}
    //public void SetMusicVolume(float value)
    //{
    //    Music = value;
    //}
    //public void SetAmbienceVolume(float value)
    //{
    //    Ambient = value;
    //}
    //public void SetEffectVolume(float value)
    //{
    //    Effects = value;
    //}
    //public void SetGameLoopVolume(float value)
    //{
    //    GameLoops = value;
    //}
    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
    public void InitializeAmbience(EventReference ambienceEventRef)
    {
        eventInstanceAmb = RuntimeManager.CreateInstance(ambienceEventRef);
        eventInstanceAmb.start();
        eventInstanceAmb.release();
    }
    public void InitializeMusic(EventReference MusicEventRef)
    {
        eventInstanceMusic = RuntimeManager.CreateInstance(MusicEventRef);//CreateInstanceOfAudio(MusicEventRef);
        eventInstanceMusic.start();
        eventInstanceMusic.release();
        //UnityEngine.Debug.Log()
    }
    public void StopMusic()
    {
        eventInstanceMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void StopAmbient()
    {
        eventInstanceAmb.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public EventInstance CreateFootstepInst(EventReference stepping)
    {
        EventInstance EI = RuntimeManager.CreateInstance(stepping);
        return EI;
    }

    public void SetMusicArea(MusicFMOD MF)
    {
        MusicInstance.setParameterByName("Area", (float)area);
    }
    public EventInstance CreateInstanceOfAudio(EventReference eventRef)
    {
        EventInstance ei = RuntimeManager.CreateInstance(eventRef);
        eventInstanses.Add(ei);
        return ei;
    }
    private void CleanList()
    {
        foreach (EventInstance ei in eventInstanses)
        {
            ei.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ei.release();
        }
    }
    private void OnDestroyMethod()
    {
        CleanList();

    }
    private void OnEnemyEnterCombat(EnemyEnterCombatEvent e)
    {
        //if(EnemiesInCombat)
        EnemiesInCombat+=0.5f;
        dealingWithenemies++;
        eventInstanceMusic.setParameterByName("Enemies", EnemiesInCombat);
        print(EnemiesInCombat + " " + dealingWithenemies);
    }
    private void OnEnemyLeaveCombat(EnemyLeaveCombatEvent e)
    {
        dealingWithenemies--;
        if(dealingWithenemies <= 0f)
            dealingWithenemies = 0f;
            eventInstanceMusic.setParameterByName("Enemies", 0f);

        print(EnemiesInCombat + " " + dealingWithenemies);
    }
}
