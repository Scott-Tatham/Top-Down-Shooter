using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : Effector
{
    int bounceCount, maxBounces;

    public void SetMaxBounces(int maxBounces)
    {
        bounceCount = 0;
        this.maxBounces = maxBounces;
    }

    public void ApplyBounce()
    {
        if (bounceCount >= maxBounces)
        {
            ammo.EffectorCall();
        }

        bounceCount++;
    }
}