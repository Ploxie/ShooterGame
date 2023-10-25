using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbiencePlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private bool playing = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!playing)
        {
            EventManager.GetInstance().TriggerEvent(new AudioLoopEvent(audioSource, "ambience"));
            playing = true;
        }
    }
}
