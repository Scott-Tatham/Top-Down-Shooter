using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assess the validity of this effector.
public class Arc : Effector
{
    public void SearchArcRadius(float radius, LayerMask layer, int maxUnits = -1)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);
    }
}