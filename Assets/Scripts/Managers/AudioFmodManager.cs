using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEditor.ShaderGraph.Internal;

public class AudioFmodManager : MonoBehaviour
{
    private List<EventInstance> eventInstanses;

    private EventInstance eventInstanceAmb;

    private EventInstance MusicInstance;
    private float area = 0;

    public static AudioFmodManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More then one audioManager");
        }
        instance = this;

        eventInstanses = new List<EventInstance>();
    }
    private void Start()
    {
        InitializeAmbience(FmodEvents.instance.ambienceTest);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
    private void InitializeAmbience(EventReference ambienceEventRef)
    {
        eventInstanceAmb = CreateInstanceOfAudio(ambienceEventRef);
        eventInstanceAmb.start();
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

}
