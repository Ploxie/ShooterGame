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

        public DamageReceivedEffect()
        {
            Name = "Slug";
        }
    }

    public class RadiationEffect : StatusEffect
    {
        public float Damage { get; set; } = 10.0f;

        public RadiationEffect()
        {
            Name = "Radiation";
        }
    }

    public class IceEffect : StatusEffect
    {
        public float MovementSpeedMultiplier { get; set; } = 0.1f;

        public IceEffect()
        {
            Name = "Ice";
        }
    }

    public class StunEffect : StatusEffect
    {
        public StunEffect()
        {
            Name = "Stun";
        }
    }

    public class DebilitationEffect : StatusEffect
    {
        public float DamageMultiplier { get; set; } = 0.5f;

        public DebilitationEffect()
        {
            Name = "Debilitation";
        }
    }
}
