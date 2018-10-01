using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    protected PrimaryFireType primaryFireType;
    [SerializeField]
    protected LayerMask impactTypes;
    [SerializeField, Range(0.01f, 5.0f)]
    protected float damageMultiplier, fireRateMultiplier, ammoRangeMultiplier;

    protected bool isFired;
    protected float baseDamage, ammoRange;
    protected Weapon weapon;

    public bool GetIsFired() { return isFired; }
    public PrimaryFireType GetPrimaryFireType() { return primaryFireType; }
    public float GetFireRateMultiplier() { return fireRateMultiplier; }

    public void SetBaseDamage(float baseDamage) { this.baseDamage = baseDamage; }
    public void SetAmmoRange(float ammoRange) { this.ammoRange = ammoRange; }
    public void SetWeapon(Weapon weapon) { this.weapon = weapon; }
}