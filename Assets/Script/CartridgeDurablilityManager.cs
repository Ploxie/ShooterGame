using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartridgeDurablilityManager : MonoBehaviour
{
    public Image weaponCartridgeBar;
    public Image effectCartridgeBar;
    public Image bulletCartridgeBar;
    public float weaponDurability = 100;
    public float effectDurability = 100;
    public float bulletDurability = 100;
    public float maxDurability = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            LoseDurability(20, 30, 40);
        }
    }

    public void LoseDurability(float weaponDurabilityLost, float effectDurabilityLost, float bulletDurabilityLost)
    {
        weaponDurability -= weaponDurabilityLost;
        weaponCartridgeBar.fillAmount = weaponDurability / maxDurability;

        effectDurability -= effectDurabilityLost;
        effectCartridgeBar.fillAmount = effectDurability / maxDurability;

        bulletDurability -= bulletDurabilityLost;
        bulletCartridgeBar.fillAmount = bulletDurability / maxDurability;
    }
}
