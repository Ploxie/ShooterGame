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
    public class SMGWeapon : Weapon
    {
        public SMGWeapon()
        {
            Name = "SMG";
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_SMG");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_78SMG");
            Description = "The SMG unleashes a hell of bullets upon your foes, killing them with a thousand cuts.";

            Data = (WeaponData)JsonConvert.DeserializeObject<WeaponData>(File.ReadAllText($"{WEAPON_DATA_PATH}/SMG.json"));
        }
    }
}
