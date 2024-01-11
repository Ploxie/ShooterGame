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
    public class ShotgunWeapon : Weapon
    {

        public ShotgunWeapon()
        {
            Name = "Shotgun";
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Shotgun");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_87Shotgun");
            Description = "Firing a wave of bullets, the shotgun is unparalled when facing many foes.";

            Data = (WeaponData)JsonConvert.DeserializeObject<WeaponData>(File.ReadAllText($"{WEAPON_DATA_PATH}/Shotgun.json"));
        }
    }
}
