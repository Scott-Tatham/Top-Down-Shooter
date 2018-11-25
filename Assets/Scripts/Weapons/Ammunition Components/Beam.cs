using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Ammo, IAmmo, IHold
{
    [SerializeField, Range(0.01f, 3.0f)]
    float beamWidth;

    bool fireTick;
    LineRenderer beam;

    void Update()
    {
        WeaponBehaviour();
    }

    public void Activate()
    {
        beam = GetComponent<LineRenderer>();
        beam.startWidth = beamWidth;
        beam.endWidth = beamWidth;
        gameObject.SetActive(true);
        isFired = true;
        BeamPositions();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        isFired = false;
    }

    public void Fire()
    {
        fireTick = true;
    }

    public void WeaponBehaviour()
    {
        if (isFired)
        {
            BeamPositions();
        }
    }

    void BeamPositions()
    {
        beam.SetPosition(0, weapon.transform.position);
        RaycastHit hit;

        if (Physics.BoxCast(weapon.transform.position, Vector3.one * beamWidth, weapon.transform.forward, out hit, Quaternion.identity, ammoRange * ammoRangeMultiplier, impactTypes))
        {
            beam.SetPosition(1, new Vector3(hit.point.x, weapon.transform.position.y, hit.point.z));
            
            if (fireTick && hit.transform.gameObject.GetComponent<Damage>())
            {
                hit.transform.gameObject.GetComponent<Damage>().ApplyDamage(baseDamage * damageMultiplier);
                fireTick = false;
            }
        }

        else
        {
            beam.SetPosition(1, weapon.transform.position + (weapon.transform.forward * ammoRange * ammoRangeMultiplier));
        }
    }
}