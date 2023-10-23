using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAudioManager : EnemyAudioManager
{
    protected override void Awake()
    {
        base.Awake();
        footstepsAudio = AudioFileManager.GetInstance().FootstepsRanged;
        deathSound = AudioFileManager.GetInstance().DeathRanged;

    }
}
