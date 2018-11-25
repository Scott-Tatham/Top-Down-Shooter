using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Effector
{
    public void ApplyDamage(float damage)
    {
        unitStats.SetCurrentHealth(unitStats.GetCurrentHealth() - damage);
    }
}