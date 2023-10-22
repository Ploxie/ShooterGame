using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAudioManager : MonoBehaviour
{
    protected AudioSource audioSource;

    [SerializeField] private List<AudioClip> footstepsAudio = new List<AudioClip>();
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        audioSource.clip = footstepsAudio[Random.Range(0, footstepsAudio.Count)];
        audioSource.Play();
    }
}
