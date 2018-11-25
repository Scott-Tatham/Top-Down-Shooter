using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : Effector
{
    Coroutine BlindTime;

    public void StartBlind(float duration)
    {
        StopCoroutine(BlindTime);
        BlindTime = StartCoroutine(ApplyBlind(duration));
    }

    IEnumerator ApplyBlind(float duration)
    {
        unitStats.SetBlind(true);
        yield return new WaitForSeconds(duration);
        unitStats.SetBlind(false);
    }
}