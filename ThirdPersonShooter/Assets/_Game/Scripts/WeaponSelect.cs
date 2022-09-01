using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{

    int currentWeaponIndex = 0;
    int nextWeaponIndex;

    private void Start() 
    {

        SetWeaponActive();
    }

    private void Update() 
    {
        int previousWeaponIndex = currentWeaponIndex;
        ProcessKeyInput();
        ProcessScrollWheelInput();
        if(previousWeaponIndex != currentWeaponIndex)
        {
            SetWeaponActive();
        }
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;
        foreach(Transform weapon in transform)
        {
            if(weaponIndex == currentWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    private void ProcessScrollWheelInput()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(currentWeaponIndex >= transform.childCount)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(currentWeaponIndex <= 0)
            {
                currentWeaponIndex = transform.childCount-1;
            }
            else
            {
                currentWeaponIndex--;
            }
        }
    }

    private void ProcessKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponIndex = 2;
        }
    }

    public AmmoType GetCurrentWeaponType()
    {
        return transform.GetChild(currentWeaponIndex).GetComponent<Gun>().GetGunAmmoType();
    }

}
