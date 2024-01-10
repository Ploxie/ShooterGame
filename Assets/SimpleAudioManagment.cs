using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleAudioManagment : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioFmodManager AFM;
    public Slider sliderMusic, sliderAmbience, sliderEffect;
    
    void Start()
    {
        AFM = FindObjectOfType<AudioFmodManager>();
    }

    public void SetMasterVolume()
    {
        //AFM.masterVolume = sliderMusic.value;
    }
    public void SetMusicVolume()
    {
        AFM.Music = sliderMusic.value; 
        AFM.GameLoops = sliderMusic.value;
    }
    public void SetAmbienceVolume()
    {
        AFM.Ambient = sliderAmbience.value;
    }
    public void SetEffectVolume()
    {
        AFM.Effects = sliderEffect.value;
    }
    // Update is called once per frame
}
