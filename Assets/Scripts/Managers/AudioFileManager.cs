using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioFileManager : Singleton<AudioFileManager>
{
    //Declare melee variables
    public List<AudioClip> FootstepsMelee { get; private set; }
    public List<AudioClip> AttackGruntMelee { get; private set; }
    public AudioClip SlamAttackMelee { get; private set; }
    public AudioClip DeathMelee { get; private set; }


    //Declare kamikaze variables
    public List<AudioClip> FootstepsKamikaze { get; private set; }
    public AudioClip ExplodeAudio { get; private set; }
    public AudioClip RoarAudio { get; private set; }

    //Declare ranged variables
    public List<AudioClip> FootstepsRanged { get; private set; }

    public AudioClip DeathRanged { get; private set; }

    private void Awake()
    {
        instance = this;
        //Load melee sound clips
        {
            FootstepsMelee = new List<AudioClip>();
            LoadAudioFromFolder("Assets/Resources/Audio/Enemy/Footsteps/Melee", FootstepsMelee);
            AttackGruntMelee = new List<AudioClip>();
            LoadAudioFromFolder("Assets/Resources/Audio/Enemy/Attack/Melee", AttackGruntMelee);
            SlamAttackMelee = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Melee/SlamAttack.mp3", typeof(AudioClip));
            DeathMelee = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Melee/Death.mp3", typeof(AudioClip));
        }
        //load kamikaze sound clips
        {
            FootstepsKamikaze = new List<AudioClip>();
            LoadAudioFromFolder("Assets/Resources/Audio/Enemy/Footsteps/Kamikaze", FootstepsKamikaze);
            ExplodeAudio = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Kamikaze/Explosion 1.mp3", typeof(AudioClip));
            RoarAudio = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Kamikaze/KamikazeRoar.mp3", typeof(AudioClip));
        }
        //load ranged sound clips
        {
            FootstepsRanged = new List<AudioClip>();
            LoadAudioFromFolder("Assets/Resources/Audio/Enemy/Footsteps/Ranged", FootstepsRanged);
            DeathRanged = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/Enemy/Misc/Ranged/Death.wav", typeof(AudioClip));
        }
    }
    protected void LoadAudioFromFolder(string path, List<AudioClip> audioList)
    {
        string[] files = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            AudioClip audio = (AudioClip)AssetDatabase.LoadAssetAtPath(file, typeof(AudioClip));
            if (audio != null)
                audioList.Add(audio);
        }
    }
}
