using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            Icon = Resources.Load<Sprite>("Resources/Sprites/Modules/Effects/Tex_skill_41Slug.PNG");
        }
    }

    public class RadiationEffect : StatusEffect
    {
        public float Damage { get; set; } = 10.0f;

        public RadiationEffect()
        {
            Name = "Radiation";
            Icon = Resources.Load<Sprite>("Resources/Sprites/Modules/Effects/Tex_skill_62Radiation.PNG");
        }
    }

    public class IceEffect : StatusEffect
    {
        public float MovementSpeedMultiplier { get; set; } = 0.1f;

        public IceEffect()
        {
            Name = "Ice";
            Icon = Resources.Load<Sprite>("Resources/Sprites/Modules/Effects/Tex_skill_31Ice.PNG");
        }
    }

    public class StunEffect : StatusEffect
    {
        public StunEffect()
        {
            Name = "Stun";
            Icon = Resources.Load<Sprite>("Resources/Sprites/Modules/Effects/Tex_skill_45Stun.PNG");
        }
    }

    public class DebilitationEffect : StatusEffect
    {
        public float DamageMultiplier { get; set; } = 0.5f;

        public DebilitationEffect()
        {
            Name = "Debilitation";
            Icon = Resources.Load<Sprite>("Resources/Sprites/Modules/Effects/Tex_skill_79Debilitation.PNG");
        }
    }
}
