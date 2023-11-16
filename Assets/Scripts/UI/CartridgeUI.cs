using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartridgeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image weapon;
    [SerializeField] private Image effect;
    [SerializeField] private Image projectile;
    void Start()
    {
        EventManager.GetInstance().AddListener<PlayerChangeModuleEvent>(SetIcons);
    }

    private void SetIcons(PlayerChangeModuleEvent e)
    {
        if (e.weapon != null)
            weapon.sprite = e.weapon.Icon;
        if (e.statusEffect != null)
            effect.sprite = e.statusEffect.Icon;
        if (e.projectile != null)
            projectile.sprite = e.projectile.Icon;
    }
}
