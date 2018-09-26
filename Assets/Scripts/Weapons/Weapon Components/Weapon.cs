using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PROJECTILE,
    ROCKET,
    SPRAY,
    BEAM
}

public enum AltFireType
{
    EMPTYCLIP,
    BUCKSHOT,
    CHUCKSHOT,
    JUMBO,
    LOBBED,
    MULTI,
    BALL,
    CANISTER,
    AREAMIST,
    AREAPULSE,
    OVERCHARGE,
    SPLIT
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    WeaponType weaponType;
    [SerializeField]
    AltFireType altFireType;

    WeaponClip weaponClip;

    public WeaponType GetWeaponType() { return weaponType; }

    void Start()
    {
        weaponClip = GetComponent<WeaponClip>();
    }

    public void PrimaryFire()
    {
        weaponClip.Fire();
    }

    public void AlternateFire()
    {

    }
}