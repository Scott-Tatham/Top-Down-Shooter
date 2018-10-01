using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrel : MonoBehaviour
{
    [SerializeField]
    AltFireType altFireType;
    [SerializeField, Range(0.01f, 100.0f)]
    float baseDamage;
    [SerializeField, Range(0.01f, 10.0f)]
    float fireRate;

    Weapon weapon;
    WeaponClip weaponClip;

    public float GetBaseDamage() { return baseDamage; }
    public float GetFireRate() { return fireRate; }

    void Awake()
    {
        weapon = GetComponent<Weapon>();
        weapon.SetAltFireType(altFireType);
    }

    void Start()
    {
        weaponClip = GetComponent<WeaponClip>();
    }
}