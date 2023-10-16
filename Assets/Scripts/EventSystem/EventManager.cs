using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.EventSystem
{
    public static class EventManager 
    {

        public static event UnityAction<float> PlayerHealthChanged;
        public static void TriggerPlayerHealthChanged(float health) => PlayerHealthChanged?.Invoke(health);

        public static event UnityAction<int> ScoreChanged;
        public static void TriggerScoreChanged(int score) => ScoreChanged?.Invoke(score);


        // Shoot Event
        public static event UnityAction ShootEvent;
        public static void TriggerShootEvent() => ShootEvent?.Invoke();


    }

}
