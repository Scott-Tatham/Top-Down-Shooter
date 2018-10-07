using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClip : MonoBehaviour
{
    [SerializeField, Range(1, 100)]
    int clipSize;
    [SerializeField, Range(0.01f, 10.0f)]
    float reloadTime;
    [SerializeField, Range(0.01f, 20.0f)]
    float ammoRange;
    [SerializeField]
    GameObject ammoObj;

    bool nextRound, reloading;
    int clipIndex;
    PrimaryFireType primaryFireType;
    AltFireType altFireType;
    float fireRateMultiplier;
    Weapon weapon;
    WeaponBarrel weaponBarrel;
    WeaponScope weaponScope;
    List<Ammo> ammo;

    void Start()
    {
        nextRound = true;
        ammo = new List<Ammo>();
        weapon = GetComponent<Weapon>();
        weaponBarrel = GetComponent<WeaponBarrel>();
        weaponScope = GetComponent<WeaponScope>();
        primaryFireType = ammoObj.GetComponent<Ammo>().GetPrimaryFireType();
        weapon.SetPrimaryFireType(primaryFireType);
        altFireType = weapon.GetAltFireType();
        fireRateMultiplier = ammoObj.GetComponent<Ammo>().GetFireRateMultiplier();

        if (primaryFireType == PrimaryFireType.PROJECTILE || primaryFireType == PrimaryFireType.ROCKET)
        {
            for (int i = 0; i < clipSize * 2; i++)
            {
                ammo.Add(Instantiate(ammoObj, Vector3.zero, Quaternion.identity).GetComponent<Ammo>());
                ammo[i].SetWeapon(weapon);
                ammo[i].gameObject.SetActive(false);
                ammo[i].SetBaseDamage(weaponBarrel.GetBaseDamage());
                ammo[i].SetAmmoRange(ammoRange);
                ammo[i].SetAccuracy(weaponScope.GetAccuracy());
            }
        }

        else
        {
            ammo.Add(Instantiate(ammoObj, Vector3.zero, Quaternion.identity).GetComponent<Ammo>());
            ammo[0].SetWeapon(weapon);
            ammo[0].gameObject.SetActive(false);
            ammo[0].SetBaseDamage(weaponBarrel.GetBaseDamage());
            ammo[0].SetAmmoRange(ammoRange);
            ammo[0].SetAccuracy(weaponScope.GetAccuracy());
        }
    }

    public void Deactivate()
    {
        if (!reloading)
        {
            if (primaryFireType == PrimaryFireType.SPRAY || primaryFireType == PrimaryFireType.BEAM)
            {
                ammo[0].GetComponent<IHold>().Deactivate();
            }
        }
    }

    public void Fire()
    {
        if (!reloading)
        {
            switch (primaryFireType)
            {
                case PrimaryFireType.PROJECTILE:
                    if (nextRound)
                    {
                        for (int i = 0; i < ammo.Count; i++)
                        {
                            if (!ammo[i].GetIsFired())
                            {
                                ammo[i].GetComponent<IAmmo>().Fire();
                                nextRound = false;
                                StartCoroutine(UseAmmo());

                                break;
                            }
                        }
                    }

                    break;

                case PrimaryFireType.ROCKET:
                    if (nextRound)
                    {
                        for (int i = 0; i < ammo.Count; i++)
                        {
                            if (!ammo[i].GetIsFired())
                            {
                                ammo[i].GetComponent<IAmmo>().Fire();
                                nextRound = false;
                                StartCoroutine(UseAmmo());

                                break;
                            }
                        }
                    }

                    break;

                case PrimaryFireType.SPRAY:
                    if (!ammo[0].GetIsFired())
                    {
                        ammo[0].GetComponent<IHold>().Activate();
                    }

                    ammo[0].GetComponent<IAmmo>().Fire();

                    if (nextRound)
                    {
                        nextRound = false;
                        StartCoroutine(UseAmmo());
                    }

                    break;

                case PrimaryFireType.BEAM:
                    if (!ammo[0].GetIsFired())
                    {
                        ammo[0].GetComponent<IHold>().Activate();
                    }

                    if (nextRound)
                    {
                        ammo[0].GetComponent<IAmmo>().Fire();
                        nextRound = false;
                        StartCoroutine(UseAmmo());
                    }

                    break;
            }

            if (clipIndex >= clipSize)
            {
                reloading = true;

                if (primaryFireType == PrimaryFireType.SPRAY || primaryFireType == PrimaryFireType.BEAM)
                {
                    ammo[0].GetComponent<IHold>().Deactivate();
                }

                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator UseAmmo()
    {
        clipIndex++;
        yield return new WaitForSeconds(weaponBarrel.GetFireRate() * fireRateMultiplier);
        nextRound = true;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        clipIndex = 0;
    }
}