using Assets.Scripts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameEvent { }

public class PlayerHealthChangeEvent : GameEvent
{
    public Health Health;

    public PlayerHealthChangeEvent(Health health)
    {
        Health = health;
    }
}

public class ScoreChangedEvent : GameEvent
{
    public int Score;

    public ScoreChangedEvent(int score)
    {
        Score = score;
    }
}

public class AudioEvent : GameEvent
{
    public AudioSource AudioSource;
    public string Key;
    public AudioEvent(AudioSource audioSource, string key)
    {
        AudioSource = audioSource;
        Key = key;
    }
}
public class AudioLoopEvent : GameEvent
{
    public AudioSource AudioSource;
    public string Key;
    public AudioLoopEvent(AudioSource audioSource, string key)
    {
        AudioSource = audioSource;
        Key = key;
    }
}

public class PlayerChangeModuleEvent : GameEvent
{
    public Weapon weapon;
    public ProjectileEffect projectile;
    public StatusEffect statusEffect;

    public PlayerChangeModuleEvent(Weapon weapon, ProjectileEffect projectile, StatusEffect statusEffect)
    {
        this.weapon = weapon;
        this.projectile = projectile;
        this.statusEffect = statusEffect;
    }
}

public class ScreenShakeEvent : GameEvent
{
    public float Intensity;
    public float Frequency;
    public float Duration;

    public ScreenShakeEvent(float intensity, float frequency, float duration)
    {
        Intensity = intensity;
        Frequency = frequency;
        Duration = duration;
    }
}
