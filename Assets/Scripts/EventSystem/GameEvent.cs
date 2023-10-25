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
