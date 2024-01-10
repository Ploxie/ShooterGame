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
        sliderMusic.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        sliderAmbience.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        sliderEffect.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
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
    public void ValueChangeCheck()
    {
        musicValue = sliderMusic.value;
        ambienceValue = sliderAmbience.value;
        effectValue = sliderEffect.value;
    }
}
