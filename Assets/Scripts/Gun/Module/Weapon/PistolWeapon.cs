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
    public class PistolWeapon : Weapon
    {

        public PistolWeapon()
        {
            Name = "Pistol";
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Pistol");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_50Pistol");
            Description = "The pistol is a reliable weapon in a pinch, but not the thing to take along if you expect any serious challenge.";

            Data = JsonConvert.DeserializeObject<WeaponData>(File.ReadAllText($"{WEAPON_DATA_PATH}/Pistol.json"));
        }

    }
}
