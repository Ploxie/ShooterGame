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
            LoadAudioFromFolder("footstepsmelee", "Assets/Resources/Audio/Enemy/Footsteps/Melee");

            LoadAudioFromFolder("attackgruntsmelee", "Assets/Resources/Audio/Enemy/Attack/Melee");

            AddSound("slammattackmelee", (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Melee/SlamAttack.mp3", typeof(AudioClip)));

            AddSound("deathmelee", (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Melee/Death.mp3", typeof(AudioClip)));

        }
        //load kamikaze sound clips
        {
            LoadAudioFromFolder("footstepskamikaze", "Assets/Resources/Audio/Enemy/Footsteps/Kamikaze");

            AddSound("explodekamikaze", (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Kamikaze/Explosion 1.mp3", typeof(AudioClip)));

            AddSound("roarkamikaze", (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Kamikaze/KamikazeRoar.mp3", typeof(AudioClip)));
        }
        //load ranged sound clips
        {
            LoadAudioFromFolder("footstepsranged", "Assets/Resources/Audio/Enemy/Footsteps/Ranged");

            AddSound("deathranged", (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Ranged/Death.wav", typeof(AudioClip)));

        }
        //load ambient sounc clips
        {
            LoadAudioFromFolder("ambience", "Assets/Resources/Audio/Ambience");
        }
    }
    private void Start()
    {
        EventManager.GetInstance().AddListener<AudioEvent>(PlaySound);
        EventManager.GetInstance().AddListener<AudioLoopEvent>(PlayAmbience);
    }
    private void LoadAudioFromFolder(string name, string path)
    {
        string[] files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            AudioClip audio = (AudioClip)AssetDatabase.LoadAssetAtPath(file, typeof(AudioClip));
            if (audio != null)
                AddSound(name, audio);
        }
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
