using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public abstract class StatusEffect : Module // Alla fungerar
    {
        public const string EFFECTS_DATA_PATH = "GameData/Effects";

        public StatusEffect()
        {
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_StatusEffect");
        }

        public StatusEffect Copy()
        {
            return (StatusEffect)MemberwiseClone();
        }

        public abstract float GetDuration();
        public abstract void SetDuration(float duration);
    }

    public class DamageReceivedEffectData
    {
        public float Duration;
        public float DamageMultiplier;
    }

    public class DamageReceivedEffect : StatusEffect // Fungerar
    {
        public DamageReceivedEffectData Data;
        //public float DamageMultiplier { get; set; } = 1.5f;

        public DamageReceivedEffect()
        {
            Name = "Slug";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_41Slug");
            Description = "Heavy slugs weaken the enemy's defences, increasing damage taken.";
            Data = JsonConvert.DeserializeObject<DamageReceivedEffectData>(File.ReadAllText($"{EFFECTS_DATA_PATH}/DamageReceived.json"));
        }

        public override float GetDuration()
        {
            return Data.Duration;
        }

        public override void SetDuration(float duration)
        {
            Data.Duration = duration;
        }
    }

    public class RadiationEffectData
    {
        public float Damage;
        public float Duration;
    }

    public class RadiationEffect : StatusEffect // Fungerar
    {
        public RadiationEffectData Data;
        //public float Damage { get; set; } = 5.0f;

        public RadiationEffect()
        {
            Name = "Radiation";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_62Radiation");
            Description = "Devastating particles inflicts radiation poisoning to the enemy, dealing damage over time.";
            Data = JsonConvert.DeserializeObject<RadiationEffectData>(File.ReadAllText($"{EFFECTS_DATA_PATH}/Radiation.json"));
            //Duration = 5;
        }

        public override float GetDuration()
        {
            return Data.Duration;
        }

        public override void SetDuration(float duration)
        {
            Data.Duration = duration;
        }
    }

    public class IceEffectData
    {
        public float Duration;
        public float MovementSpeedMultiplier;
    }

    public class IceEffect : StatusEffect //Fungerar
    {

        public IceEffectData Data;
        //public float MovementSpeedMultiplier { get; set; } = 0.5f;

        public IceEffect()
        {
            Name = "Ice";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_31Ice");
            Description = "Superchilled gasses chills the enemy to its bones, slowing it down.";
            Data = JsonConvert.DeserializeObject<IceEffectData>(File.ReadAllText($"{EFFECTS_DATA_PATH}/Ice.json"));
        }

        public override float GetDuration()
        {
            return Data.Duration;
        }

        public override void SetDuration(float duration)
        {
            Data.Duration = duration;
        }
    }

    public class StunEffectData
    {
        public float Duration;
    }

    public class StunEffect : StatusEffect //Fungerar
    {
        public StunEffectData Data;

        public StunEffect()
        {
            Name = "Stun";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_45Stun");
            Description = "An electrical shock causes the enemy's nervous systems to go haywire, stunning them for a brief period of time.";
            Data = JsonConvert.DeserializeObject<StunEffectData>(File.ReadAllText($"{EFFECTS_DATA_PATH}/Stun.json"));
            //Duration = 0.5f;
        }

        public override float GetDuration()
        {
            return Data.Duration;
        }

        public override void SetDuration(float duration)
        {
            Data.Duration = duration;
        }
    }

    public class DebilitationEffectData
    {
        public float DamageMultiplier;
        public float Duration;
    }

    public class DebilitationEffect : StatusEffect //Fungerar
    {
        public DebilitationEffectData Data;
        //public float DamageMultiplier { get; set; } = 0.5f;

        public DebilitationEffect()
        {
            Name = "Debilitation";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Effects/Tex_skill_79Debilitation");
            Description = "Viral agents infect the enemy, decreasing its damage dealt.";
            Data = JsonConvert.DeserializeObject<DebilitationEffectData>(File.ReadAllText($"{EFFECTS_DATA_PATH}/Debilitation.json"));
            //Duration = 5f;
        }

        public override float GetDuration()
        {
            return Data.Duration;
        }

        public override void SetDuration(float duration)
        {
            Data.Duration = duration;
        }
    }
}
