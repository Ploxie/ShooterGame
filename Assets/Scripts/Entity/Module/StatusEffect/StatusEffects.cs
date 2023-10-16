using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity
{
    public abstract class StatusEffect : Module
    {
        public float Duration { get; set; } = 1.0f;

        public StatusEffect Copy()
        {
            return (StatusEffect)MemberwiseClone();
        }
    }

    public class DamageReceivedEffect : StatusEffect
    {
        public float DamageMultiplier { get; set; } = 1.5f;
    }

    public class RadiationEffect : StatusEffect
    {
        public float Damage { get; set; } = 10.0f;
    }

    public class IceEffect : StatusEffect
    {
        public float MovementSpeedMultiplier { get; set; } = 0.1f;
    }

    public class StunEffect : StatusEffect
    {

    }

    public class DebilitationEffect : StatusEffect
    {
        public float DamageMultiplier { get; set; } = 0.5f;
    }
}
