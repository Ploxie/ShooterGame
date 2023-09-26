using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int HitPoints;
    [SerializeField] public int MaxHitPoints;

    [Header("Events")]
    [SerializeField] public GameEvent OnUnitHealthChange;
    [SerializeField] public GameEvent OnUnitDeath;

    private void Start()
    {
        HitPoints = MaxHitPoints;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(MaxHitPoints);

        if (HitPoints <= 0)
            OnUnitDeath.Raise(this, 0);
    }

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;

        OnUnitHealthChange.Raise(this, HitPoints);
    }
}
