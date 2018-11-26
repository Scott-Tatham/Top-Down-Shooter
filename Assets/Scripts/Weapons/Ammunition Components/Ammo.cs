using LazyTitan.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    protected PrimaryFireType primaryFireType;
    [SerializeField, EnumBitMask]
    protected EffectorType effectorTypes;
    [SerializeField]
    protected LayerMask impactTypes;
    [SerializeField, Range(0.01f, 5.0f)]
    protected float damageMultiplier, fireRateMultiplier, ammoRangeMultiplier;

    protected bool isFired;
    protected float baseDamage, ammoRange, accuracy;
    protected EffectEvent OnFire, OnEffector, OnEnemyContact, OnEnvironmentContact;
    protected Weapon weapon;
    protected Effector[] effectors;

    protected delegate void EffectEvent();

    public bool GetIsFired() { return isFired; }
    public PrimaryFireType GetPrimaryFireType() { return primaryFireType; }
    public float GetFireRateMultiplier() { return fireRateMultiplier; }

    public void SetBaseDamage(float baseDamage) { this.baseDamage = baseDamage; }
    public void SetAmmoRange(float ammoRange) { this.ammoRange = ammoRange; }
    public void SetAccuracy(float accuracy) { this.accuracy = accuracy; }
    public void SetWeapon(Weapon weapon) { this.weapon = weapon; }

    void Start()
    {
        effectors = GetComponents<Effector>();
    }

    public void FiredCall()
    {
        OnFire();
    }

    public void EffectorCall()
    {
        OnEffector();
    }

    public void EnemyContactCall()
    {
        OnEnemyContact();
    }

    public void EnvironmentContactCall()
    {
        OnEnvironmentContact();
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        // If it has effector types make it's call.
    }
}