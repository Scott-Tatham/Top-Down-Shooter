using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOT : Effector
{
    Coroutine HOTTime;

    public void StartHOT(float heal, int ticks, float intervals)
    {
        StopCoroutine(HOTTime);
        HOTTime = StartCoroutine(ApplyHOT(heal, ticks, intervals));
    }

    IEnumerator ApplyHOT(float heal, int ticks, float intervals)
    {
        for (int i = 0; i < ticks; i++)
        {
            if (unitStats.GetCurrentHealth() + heal <= unitStats.GetInitialHealth())
            {
                unitStats.SetCurrentHealth(unitStats.GetCurrentHealth() + heal);
            }

            else
            {
                unitStats.SetCurrentHealth(unitStats.GetInitialHealth());
            }

            yield return new WaitForSeconds(intervals);
        }
    }
}