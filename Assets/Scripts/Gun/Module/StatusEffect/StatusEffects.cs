using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public abstract class StatusEffect : Module // Alla fungerar
    {
        public float Duration { get; set; } = 5.0f;

        public StatusEffect()
        {
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_StatusEffect");
        }

        public StatusEffect Copy()
        {
            return (StatusEffect)MemberwiseClone();
        }
    }

    public class DamageReceivedEffect : StatusEffect // Fungerar
    {
        public float DamageMultiplier { get; set; } = 1.5f;

        public DamageReceivedEffect()
        {
            Name = "Slug";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_41Slug");
            Description = "Heavy slugs weaken the enemy's defences, increasing damage taken.";
        }
    }

    public class RadiationEffect : StatusEffect // Fungerar
    {
        public float Damage { get; set; } = 5.0f;

        public RadiationEffect()
        {
            Name = "Radiation";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_62Radiation");
            Description = "Devastating particles inflicts radiation poisoning to the enemy, dealing damage over time.";
        }
    }

    public class IceEffect : StatusEffect //Fungerar
    {
        public float MovementSpeedMultiplier { get; set; } = 0.1f;

        public IceEffect()
        {
            Name = "Ice";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_31Ice");
            Description = "Superchilled gasses chills the enemy to its bones, slowing it down.";
        }
    }

    public class StunEffect : StatusEffect //Fungerar
    {
        public StunEffect()
        {
            Name = "Stun";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_45Stun");
            Description = "An electrical shock causes the enemy's nervous systems to go haywire, stunning them for a brief period of time.";
        }
    }

    public class DebilitationEffect : StatusEffect //Fungerar
    {
        public float DamageMultiplier { get; set; } = 0.5f;

        public DebilitationEffect()
        {
            Name = "Debilitation";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_79Debilitation");
            Description = "Viral agents infect the enemy, decreasing its damage dealt.";
        }
    }
}
