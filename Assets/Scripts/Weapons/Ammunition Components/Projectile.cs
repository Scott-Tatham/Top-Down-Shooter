using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ammo, IAmmo
{
    [SerializeField, Range(0.01f, 10.0f)]
    float projectileLife;

    void Update()
    {
        WeaponBehaviour();
    }

    public void Fire()
    {
        gameObject.SetActive(true);
        isFired = true;
        transform.position = weapon.transform.position;
        transform.rotation = weapon.transform.rotation;
        transform.Rotate(Vector3.up, Random.Range(-90.0f, 90.0f) * (1.0f - accuracy));

        StartCoroutine(ProjectileLife());
    }

    public void WeaponBehaviour()
    {
        if (isFired)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, ammoRange * ammoRangeMultiplier * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (((1 << col.gameObject.layer) | impactTypes.value) == impactTypes.value)
        {
            if (col.gameObject.GetComponent<Damage>())
            {
                col.gameObject.GetComponent<Damage>().ApplyDamage(baseDamage * damageMultiplier);
            }

            gameObject.SetActive(false);
            isFired = false;
        }
    }

    IEnumerator ProjectileLife()
    {
        yield return new WaitForSeconds(projectileLife);
        gameObject.SetActive(false);
        isFired = false;
    }
}