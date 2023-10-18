using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance;

    protected Dictionary<int, Living> enemies;

    public Player Player { get; private set; }

    private void Awake()
    {
        enemies = new Dictionary<int, Living>();
        Instance = this;
    }

    private void Update()
    {
        if (Player != null)
            return;

        var foundPlayer = FindObjectOfType<Player>();
        if (foundPlayer != null)
            Player = foundPlayer;
    }

    public void RegisterEnemy(Living enemy)
    {
        enemies.Add(enemy.LivingID, enemy);
    }

    public void PurgeEnemy(Living enemy)
    {
        enemies.Remove(enemy.LivingID);
    }

    public Living GetClosestEnemy(Vector3 point)
    {
        float closestDistance = float.MaxValue;
        Living closest = null;
        //Could be optimized
        foreach (Living enemy in enemies.Values)
        {
            float distance = Vector3.Distance(point, enemy.transform.position);
            if (distance > closestDistance)
                continue;

            closestDistance = distance;
            closest = enemy;
        }

        return closest;
    }

    public List<Living> GetEnemiesInRange(Vector3 point, float range)
    {
        List<Living> inRange = new List<Living>();
        foreach (Living enemy in enemies.Values)
        {
            float distance = Vector3.Distance(point, enemy.transform.position);
            if (distance > range)
                continue;

            inRange.Add(enemy);
        }

        return inRange;
    }
}