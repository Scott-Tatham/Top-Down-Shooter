using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Effector
{
    public void ApplyHeal(float heal)
    {
        if (unitStats.GetCurrentHealth() + heal <= unitStats.GetInitialHealth())
        {
            unitStats.SetCurrentHealth(unitStats.GetCurrentHealth() + heal);
        }

        else
        {
            unitStats.SetCurrentHealth(unitStats.GetInitialHealth());
        }
    }
}