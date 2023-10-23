using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class EnemyAudioManager : MonoBehaviour
{
    protected AudioSource audioSource;

    [SerializeField] protected List<AudioClip> footstepsAudio = new List<AudioClip>();

    protected AudioClip deathSound;
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        audioSource.PlayOneShot(footstepsAudio[Random.Range(0, footstepsAudio.Count)], 1f);
    }

    public void PlayDeathSound()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        //audioSource.PlayOneShot(deathSound, 1f);
    }
}
