using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryFireType
{
    PROJECTILE,
    ROCKET,
    SPRAY,
    BEAM
}

public enum AltFireType
{
    ALTONE,
    ALTTWO,
    ALTTHREE
}

public class Weapon : MonoBehaviour
{
    PrimaryFireType primaryFireType;
    AltFireType altFireType;

    WeaponClip weaponClip;
    WeaponBarrel weaponBarrel;

    public PrimaryFireType GetPrimaryFireType() { return primaryFireType; }
    public AltFireType GetAltFireType() { return altFireType; }

    public void SetPrimaryFireType(PrimaryFireType primaryFireType) { this.primaryFireType = primaryFireType; }
    public void SetAltFireType(AltFireType altFireType) { this.altFireType = altFireType; }

    void Start()
    {
        weaponClip = GetComponent<WeaponClip>();
        weaponBarrel = GetComponent<WeaponBarrel>();
    }

    public void DeactivatePrimaryFire()
    {
        weaponClip.Deactivate();
    }

    public void PrimaryFire()
    {
        weaponClip.Fire();
    }

    public void AlternateFire()
    {

    }
}