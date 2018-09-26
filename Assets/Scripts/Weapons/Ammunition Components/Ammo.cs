using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    protected bool isFired;
    protected Weapon weapon;

    public bool GetIsFired() { return isFired; }
    public void SetWeapon(Weapon weapon) { this.weapon = weapon; }
}