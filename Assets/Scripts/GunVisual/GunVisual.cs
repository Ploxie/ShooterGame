using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject baseModel;
    [SerializeField]
    GameObject automaticBarrel;
    [SerializeField]
    GameObject boltActionBarrel;
    [SerializeField]
    GameObject pistolBarrel;
    [SerializeField]
    GameObject shotGunBarrel;
    [SerializeField]
    GameObject smgBarrel;

    private void Awake()
    {

    }

    public void UpdateVisuals(WeaponModule weaponModule)
    {
        foreach (Transform child in baseModel.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (weaponModule is AutomaticModule)
        {
            automaticBarrel.SetActive(true);
        }
        else if (weaponModule is BoltActionModule)
        {
            boltActionBarrel.SetActive(true);
        }
        else if (weaponModule is PistolModule)
        {
            pistolBarrel.SetActive(true);
        }
        else if (weaponModule is ShotgunModule)
        {
            shotGunBarrel.SetActive(true);
        }
        else if (weaponModule is SMGModule)
        {
            smgBarrel.SetActive(true);
        }
    }


}
