using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : Ammo, IAmmo, IHold
{
    bool sprayEnd;
    ParticleSystem spray;

    void Update()
    {
        WeaponBehaviour();

        if (sprayEnd)
        {
            if (spray.particleCount <= 0)
            {
                gameObject.SetActive(false);
                sprayEnd = false;
            }
        }
    }

    public void Activate()
    {
        spray = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = new ParticleSystem.MainModule();
        main = spray.main;
        main.startLifetime = ammoRange * ammoRangeMultiplier;
        ParticleSystem.ShapeModule shape = new ParticleSystem.ShapeModule();
        shape = spray.shape;
        shape.angle = 90 * (1.0f - accuracy);
        gameObject.SetActive(true);
        spray.Play();
        isFired = true;
        transform.position = weapon.transform.position;
        transform.rotation = weapon.transform.rotation;
    }

    public void Deactivate()
    {
        spray.Stop();
        isFired = false;
        sprayEnd = true;
    }

    public void Fire()
    {

    }

    public void WeaponBehaviour()
    {
        if (isFired)
        {
            transform.position = weapon.transform.position;
            transform.rotation = weapon.transform.rotation;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Damage>())
        {
            other.GetComponent<Damage>().ApplyDamage(baseDamage * damageMultiplier);
        }
    }
}