using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioFileManager : MonoBehaviour
{
    private static Dictionary<string, List<AudioClip>> audioClips;

    private static AudioFileManager instance;

    public float MasterLevel { get; private set; } = 1f;
    public float AmbienceLevel { get; private set; } = 0.5f;
    public float EffectLevel { get; private set; } = 1;
    public static AudioFileManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
        if (audioClips != null)
            return;
        audioClips = new Dictionary<string, List<AudioClip>>();
        //Load melee sound clips
        {
            LoadAudioFromFolder("footstepsmelee", "Audio/Enemy/Footsteps/Melee");

            LoadAudioFromFolder("attackgruntsmelee", "Audio/Enemy/Attack/Melee");

            AddSound("slammattackmelee", Resources.Load<AudioClip>("Audio/Enemy/Misc/Melee/SlamAttack.mp3"));

            AddSound("deathmelee", Resources.Load<AudioClip>("Audio/Enemy/Misc/Melee/Death.mp3"));

        }
        //load kamikaze sound clips
        {
            LoadAudioFromFolder("footstepskamikaze", "Audio/Enemy/Footsteps/Kamikaze");

            AddSound("explodekamikaze", Resources.Load<AudioClip>("Audio/Enemy/Misc/Kamikaze/Explosion 1.mp3"));

            AddSound("roarkamikaze", Resources.Load<AudioClip>("Audio/Enemy/Misc/Kamikaze/KamikazeRoar.mp3"));
        }
        //load ranged sound clips
        {
            LoadAudioFromFolder("footstepsranged", "Audio/Enemy/Footsteps/Ranged");

            AddSound("deathranged", Resources.Load<AudioClip>("Audio/Enemy/Misc/Ranged/Death.wav"));

        }
        //load ambient sounc clips
        {
            LoadAudioFromFolder("ambience", "Audio/Ambience");
        }
    }
    private void Start()
    {
        EventManager.GetInstance().AddListener<AudioEvent>(PlaySound);
        EventManager.GetInstance().AddListener<AudioLoopEvent>(PlayAmbience);
    }
    private void LoadAudioFromFolder(string name, string path)
    {
       
        //foreach (var file in files)
        //{


        //    if (audio != null)
        //        AddSound(name, audio);
        //}
        audioClips.Add(name, new List<AudioClip>(Resources.LoadAll<AudioClip>(path)));
    }
    private void AddSound(string name, AudioClip audioClip)
    {
        string key = name.ToLower();
        if (audioClips.TryGetValue(key, out List<AudioClip> list))
        {
            list.Add(audioClip);
        }
        else
        {
            audioClips.Add(key, new List<AudioClip> { audioClip });
        }

    }

    public void PlaySound(AudioEvent e)
    {
        string key = e.Key.ToLower();
        if (audioClips.ContainsKey(key))
        {
            List<AudioClip> tempList = audioClips[key];
            AudioClip tempClip;
            if (tempList.Count > 1)
            {
                tempClip = tempList[Random.Range(0, tempList.Count)];
            }
            else
            {
                tempClip = tempList[0];
            }
            if (key.Contains("death"))
            {
                e.AudioSource.clip = tempClip;
                e.AudioSource.volume = MasterLevel * EffectLevel;
                e.AudioSource.Play();
            }
            else
            {
                e.AudioSource.PlayOneShot(tempClip, MasterLevel * EffectLevel);
            }
        }
    }
    public void PlayAmbience(AudioLoopEvent e)
    {
        string key = e.Key.ToLower();
        if (audioClips.ContainsKey(key))
        {
            List<AudioClip> tempList = audioClips[key];
            AudioClip tempClip;
            if (tempList.Count > 1)
            {
                tempClip = tempList[Random.Range(0, tempList.Count)];
            }
            else
            {
                tempClip = tempList[0];
            }

            e.AudioSource.clip = tempClip;
            e.AudioSource.volume = MasterLevel * AmbienceLevel;
            e.AudioSource.loop = true;
            e.AudioSource.Play();

        }
    }
}
