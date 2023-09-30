using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BarrelData
{
    public GameObject BarrelObject;
    public GameObject FiringPosition;
}

public class GunVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject baseModel;

    public List<BarrelData> Barrels;

    private WeaponType selectedType;

    public Vector3 GetBarrelPosition()
    {
        return Barrels[(int)selectedType].FiringPosition.transform.position;
    }

    public void UpdateVisuals(WeaponType weaponType)
    {
        foreach (Transform child in baseModel.transform)
        {
            child.gameObject.SetActive(false);
        }

        selectedType = weaponType;
        Barrels[(int)selectedType].BarrelObject.SetActive(true);
    }


}
