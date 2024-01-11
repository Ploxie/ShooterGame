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
    public class AssaultRifleWeapon : Weapon
    {

        public AssaultRifleWeapon()
        {
            Name = "Assault Rifle";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_51AssaultRifle");
            Description = "A perfect balance of speed and damage, the assault rifle can adapt to any situation.";

            Data = (WeaponData)JsonConvert.DeserializeObject<WeaponData>(File.ReadAllText($"{WEAPON_DATA_PATH}/AssaultRifle.json"));
        }

    }
}
