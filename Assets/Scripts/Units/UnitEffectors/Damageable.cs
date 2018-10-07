using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    UnitStats unitStats;

    void Start()
    {
        unitStats = GetComponent<UnitStats>();
    }

    public void DealDamage(float damage)
    {
        unitStats.SetHealth(unitStats.GetHealth() - damage);
    }
}