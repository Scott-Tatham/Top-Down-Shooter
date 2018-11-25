using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT : Effector
{
    Coroutine DOTTime;

    public void StartDOT(float damage, int ticks, float intervals)
    {
        StopCoroutine(DOTTime);
        DOTTime = StartCoroutine(ApplyDOT(damage, ticks, intervals));
    }

    IEnumerator ApplyDOT(float damage, int ticks, float intervals)
    {
        for (int i = 0; i < ticks; i++)
        {
            unitStats.SetCurrentHealth(unitStats.GetCurrentHealth() - damage);

            yield return new WaitForSeconds(intervals);
        }
    }
}