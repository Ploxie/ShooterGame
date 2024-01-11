using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class SniperWeapon : Weapon
    {
        public SniperWeapon()
        {
            Name = "Sniper";
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Sniper");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_65Sniper");
            Description = "The sniper rifle packs a powerful punch from a long distance, but is lacking in the fire rate department.";

            Data = (WeaponData)JsonConvert.DeserializeObject<WeaponData>(File.ReadAllText($"{WEAPON_DATA_PATH}/Sniper.json"));
        }

    }
}
