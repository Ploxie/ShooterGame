using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioManagment : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioFmodManager AFM;
    
    void Start()
    {
        AFM = FindObjectOfType<AudioFmodManager>();
    }

    public void SetMasterVolume(float value)
    {
        AFM.masterVolume = value;
    }
    public void SetMusicVolume(float value)
    {
        AFM.Music = value;
    }
    public void SetAmbienceVolume(float value)
    {
        AFM.Ambient = value;
        AFM.GameLoops = value;
    }
    public void SetEffectVolume(float value)
    {
        AFM.Effects = value;
    }
    // Update is called once per frame
}
