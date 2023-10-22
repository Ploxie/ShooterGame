using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyAudioManager : EnemyAudioManager
{
    [SerializeField] private AudioClip explodeAudio;
    [SerializeField] private AudioClip roarAudio;

    public void PlayExplosion()
    {
        audioSource.clip = explodeAudio;
        audioSource.Play();
    }
    public void PlayRoar()
    {
        audioSource.clip = roarAudio;
        audioSource.Play();
    }

}
