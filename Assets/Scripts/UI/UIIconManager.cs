using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIconManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject weaponParent;
    [SerializeField] private GameObject effectParent;
    [SerializeField] private GameObject bulletParent;
    UIIcon[] weaponIcons;
    UIIcon[] effectIcons;
    UIIcon[] bulletIcons;
    private Player player;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void OnEnable()
    {
        player = FindObjectOfType<Player>();
        UpdateIcons();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
        public void UpdateIcons()
    {
        weaponIcons = weaponParent.GetComponentsInChildren<UIIcon>();
        foreach (UIIcon icon in weaponIcons)
        {
            icon.description = description;
        }
        effectIcons = effectParent.GetComponentsInChildren<UIIcon>();
        foreach (UIIcon icon in effectIcons)
        {
            icon.description = description;
        }
        bulletIcons = bulletParent.GetComponentsInChildren<UIIcon>();
        foreach (UIIcon icon in bulletIcons)
        {
            icon.description = description;
        }
        int moduleIndex = 0;
        Weapon[] weapons = player.GetWeapons().GetArray();
        foreach (UIIcon icon in weaponIcons)
        {
            Weapon weapon = weapons[moduleIndex];
            if (weapon != null)
            {
                icon.SetModule(weapon);
            }
            moduleIndex++;

        }
        moduleIndex = 0;
        StatusEffect[] effects = player.GetEffects().GetArray();
        foreach (UIIcon icon in effectIcons)
        {
            StatusEffect effect = effects[moduleIndex];
            if (effect != null)
            {
                icon.SetModule(effect);
            }
            moduleIndex++;

        }
        moduleIndex = 0;
        ProjectileEffect[] projectiles = player.GetBullets().GetArray();
        foreach (UIIcon icon in bulletIcons)
        {
            ProjectileEffect projectile = projectiles[moduleIndex];
            if (projectile != null)
            {
                icon.SetModule(projectile);
            }
            moduleIndex++;

        }

    }
}
