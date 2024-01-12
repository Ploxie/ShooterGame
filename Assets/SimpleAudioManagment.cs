using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleAudioManagment : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioFmodManager AFM;
    public Slider sliderMusic, sliderAmbience, sliderEffect;
    public static float musicValue, ambienceValue, effectValue;
    
    void Start()
    {
        AFM = FindObjectOfType<AudioFmodManager>();
        sliderMusic.onValueChanged.AddListener(delegate { ValueChangeCheckMusic(); });
        sliderAmbience.onValueChanged.AddListener(delegate { ValueChangeCheckAmbience(); });
        sliderEffect.onValueChanged.AddListener(delegate { ValueChangeCheckEffect(); });
        sliderMusic.value = musicValue;
        sliderAmbience.value = ambienceValue;
        sliderEffect.value = effectValue;
    }

    private void Update()
    {
        Debug.Log(musicValue);
    }

    public void SetMasterVolume()
    {
        //AFM.masterVolume = sliderMusic.value;
    }
    public void SetMusicVolume()
    {
        AFM.Music = musicValue; 
        AFM.GameLoops = musicValue;
    }
    public void SetAmbienceVolume()
    {
        AFM.Ambient = ambienceValue;
    }
    public void SetEffectVolume()
    {
        AFM.Effects = effectValue;
    }
    public void ValueChangeCheckMusic()
    {
        musicValue = sliderMusic.value;
    }
    public void ValueChangeCheckAmbience()
    {
        ambienceValue = sliderAmbience.value;
    }
    public void ValueChangeCheckEffect()
    {
        effectValue = sliderEffect.value;
    }
}
