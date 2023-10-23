using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MeleeEnemyAudioManager : EnemyAudioManager
{
    private List<AudioClip> attackGrunts;
    private AudioClip slamAttack;
    protected override void Awake()
    {
        base.Awake();
        footstepsAudio = AudioFileManager.GetInstance().FootstepsMelee;
        attackGrunts = AudioFileManager.GetInstance().AttackGruntMelee;
        slamAttack = AudioFileManager.GetInstance().SlamAttackMelee;
        deathSound = AudioFileManager.GetInstance().DeathMelee;
    }
    public void PlayAttackGrunt()
    {
        //audioSource.clip = attackGrunts[Random.Range(0, attackGrunts.Count)];
        //audioSource.Play();
        audioSource.PlayOneShot(attackGrunts[Random.Range(0, attackGrunts.Count)], 1f);
    }
    public void PlaySlamAttack()
    {
        //audioSource.clip = slamAttack;
        //audioSource.Play();
        audioSource.PlayOneShot(slamAttack, 1f);
    }
}
