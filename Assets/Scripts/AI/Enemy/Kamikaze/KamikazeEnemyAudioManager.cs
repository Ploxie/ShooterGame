using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyAudioManager : EnemyAudioManager
{
    private AudioClip explodeAudio;
    private AudioClip roarAudio;

    protected override void Awake()
    {
        base.Awake();
        footstepsAudio = AudioFileManager.GetInstance().FootstepsKamikaze;
        explodeAudio = AudioFileManager.GetInstance().ExplodeAudio;
        roarAudio = AudioFileManager.GetInstance().RoarAudio;
    }

    public void PlayExplosion()
    {
        audioSource.PlayOneShot(explodeAudio, 1f);
        //audioSource.clip = explodeAudio;
        //audioSource.Play();
    }
    public void PlayRoar()
    {
        audioSource.PlayOneShot(roarAudio, 1f);
        //audioSource.clip = roarAudio;
        //audioSource.Play();
    }

}
