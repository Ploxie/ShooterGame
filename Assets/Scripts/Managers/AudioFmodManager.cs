using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEditor.ShaderGraph.Internal;
using Assets.Scripts.Entity;

public class AudioFmodManager : MonoBehaviour
{
    private List<EventInstance> eventInstanses;

    private EventInstance eventInstanceAmb, eventInstanceMusic;

    private EventInstance MusicInstance;
    private float area = 0;

    public static AudioFmodManager instance { get; private set; }
    private int EnemiesInCombat = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More then one audioManager");
        }
        instance = this;

        eventInstanses = new List<EventInstance>();

        EventManager.GetInstance().AddListener<EnemyEnterCombatEvent>(OnEnemyEnterCombat);
        EventManager.GetInstance().AddListener<EnemyLeaveCombatEvent>(OnEnemyLeaveCombat);

    }
    private void Start()
    {
        
        //InitializeAmbience(FmodEvents.instance.ambienceTest);
        //InitializeMusic(FmodEvents.instance.MusicLoop);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
    public void InitializeAmbience(EventReference ambienceEventRef)
    {
        eventInstanceAmb = CreateInstanceOfAudio(ambienceEventRef);
        eventInstanceAmb.start();
    }
    public void InitializeMusic(EventReference MusicEventRef)
    {
        eventInstanceMusic = CreateInstanceOfAudio(MusicEventRef);
        eventInstanceMusic.start();
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
        EnemiesInCombat++;
        eventInstanceMusic.setParameterByName("Enemies", (float)EnemiesInCombat);
    }
    private void OnEnemyLeaveCombat(EnemyLeaveCombatEvent e)
    {
        if(EnemiesInCombat > 0)
            EnemiesInCombat--;
        eventInstanceMusic.setParameterByName("Enemies", (float)EnemiesInCombat);
    }
}
