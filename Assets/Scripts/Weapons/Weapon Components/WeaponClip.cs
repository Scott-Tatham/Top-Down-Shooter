using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClip : MonoBehaviour
{
    [SerializeField]
    int clipSize;
    [SerializeField, Range(0.01f, 10.0f)]
    float fireRate, reloadTime;
    [SerializeField]
    GameObject ammoObj;

    bool nextRound, reloading;
    int clipIndex;
    WeaponType weaponType;
    Weapon weapon;
    List<Ammo> ammo;

    void Start()
    {
        nextRound = true;
        ammo = new List<Ammo>();
        weapon = GetComponent<Weapon>();
        weaponType = weapon.GetWeaponType();

        for (int i = 0; i < clipSize * 2; i++)
        {
            ammo.Add(Instantiate(ammoObj, Vector3.zero, Quaternion.identity).GetComponent<Ammo>());
            ammo[i].SetWeapon(weapon);
            ammo[i].gameObject.SetActive(false);
        }
    }

    public void Fire()
    {
        if (!reloading)
        {
            switch (weaponType)
            {
                case WeaponType.PROJECTILE:
                    if (nextRound)
                    {
                        for (int i = 0; i < ammo.Count; i++)
                        {
                            if (!ammo[i].GetIsFired())
                            {
                                ammo[i].GetComponent<IAmmo>().Fire();
                                clipIndex++;
                                nextRound = false;
                                StartCoroutine(NextRouund());

                                break;
                            }
                        }
                    }

                    break;

                case WeaponType.ROCKET:
                    if (nextRound)
                    {
                        for (int i = 0; i < ammo.Count; i++)
                        {
                            if (!ammo[i].GetIsFired())
                            {
                                ammo[i].GetComponent<IAmmo>().Fire();
                                clipIndex++;
                                nextRound = false;
                                StartCoroutine(NextRouund());

                                break;
                            }
                        }
                    }

                    break;

                case WeaponType.SPRAY:
                    ammo[0].GetComponent<IAmmo>().Fire();
                    StartCoroutine(UseAmmo());

                    break;

                case WeaponType.BEAM:
                    ammo[0].GetComponent<IAmmo>().Fire();
                    StartCoroutine(UseAmmo());

                    break;
            }

            if (clipIndex >= clipSize)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator NextRouund()
    {
        yield return new WaitForSeconds(fireRate);
        nextRound = true;
    }

    IEnumerator UseAmmo()
    {
        yield return new WaitForSeconds(fireRate);
        clipIndex++;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        clipIndex = 0;
    }
}