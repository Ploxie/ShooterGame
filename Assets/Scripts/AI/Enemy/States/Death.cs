
using Assets.Scripts.Entity;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic/Death")]
public class Death : State
{
    protected Enemy enemy;
    protected double deathTimerStarted;
    protected double deathTimerDuration = 5000;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (Enemy)parent;

    }
    public override void ChangeState()
    {
        
    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {

        if (Utils.GetUnixMillis() - deathTimerStarted >= deathTimerDuration)
        {
            //if (enemy.Player.inWaveRoom == true)//Ska göra detta bara om man är i rummet med waves
            //{
            //    enemy.waveSpawner.Waves[enemy.waveSpawner.CurrentWaveIndex].EnemiesLeft--;
            //}
            Destroy(enemy.gameObject);
        }
    }

    public override void Enter()
    {
        EventManager.GetInstance().TriggerEvent(new EnemyLeaveCombatEvent());
        deathTimerStarted = Utils.GetUnixMillis();
        if (!enemy.SimulationEnabled)
            enemy.Agent.isStopped = true;
        enemy.GetComponent<Collider>().enabled = false;
    }
}
