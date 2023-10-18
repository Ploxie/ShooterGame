using Assets.Scripts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
