using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is garbage, replace later
public enum WeaponType
{
    Automatic,
    BoltAction,
    Pistol,
    Shotgun,
    SMG
}

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

    //Please for the love of god fix this later //Hampus
    public GameObject AutomaticFiringPosition;
    public GameObject BoltFiringPosition;
    public GameObject PistolFiringPosition;
    public GameObject ShotgunFiringPosition;
    public GameObject SMGFiringPosition;


    private WeaponType selectedType;

    private void Awake()
    {

    }

    //Replace with proper system in production //Hampus
    public Vector3 GetBarrelPosition()
    {
        switch (selectedType)
        {
            case WeaponType.Automatic:
                return AutomaticFiringPosition.transform.position;
            case WeaponType.BoltAction:
                return BoltFiringPosition.transform.position;
            case WeaponType.Pistol:
                return PistolFiringPosition.transform.position;
            case WeaponType.Shotgun:
                return ShotgunFiringPosition.transform.position;
            case WeaponType.SMG:
                return SMGFiringPosition.transform.position;
            default:
                return Vector3.zero;
        }
    }

    public void UpdateVisuals(WeaponModule weaponModule)
    {
        foreach (Transform child in baseModel.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (weaponModule is AutomaticModule)
        {
            selectedType = WeaponType.Automatic;
            automaticBarrel.SetActive(true);
        }
        else if (weaponModule is BoltActionModule)
        {
            selectedType = WeaponType.BoltAction;
            boltActionBarrel.SetActive(true);
        }
        else if (weaponModule is PistolModule)
        {
            selectedType = WeaponType.Pistol;
            pistolBarrel.SetActive(true);
        }
        else if (weaponModule is ShotgunModule)
        {
            selectedType = WeaponType.Shotgun;
            shotGunBarrel.SetActive(true);
        }
        else if (weaponModule is SMGModule)
        {
            selectedType = WeaponType.SMG;
            smgBarrel.SetActive(true);
        }
    }


}
