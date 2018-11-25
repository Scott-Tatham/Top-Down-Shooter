using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotation is off now until models come in.
public class Rocket : Ammo, IAmmo
{
    [SerializeField, Range(0.01f, 10.0f)]
    float rocketLife, impactRadius;

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
        transform.Rotate(90, 0, 0, Space.Self);
        transform.Rotate(Vector3.forward, Random.Range(-90.0f, 90.0f) * (1.0f - accuracy));

        StartCoroutine(RocketLife());
    }

    public void WeaponBehaviour()
    {
        if (isFired)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, ammoRange * ammoRangeMultiplier * Time.deltaTime);
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (((1 << col.gameObject.layer) | impactTypes.value) == impactTypes.value)
        {
            gameObject.SetActive(false);
            isFired = false;

            Collider[] impactCols = Physics.OverlapSphere(col.contacts[0].point, impactRadius, impactTypes);

            for (int i = 0; i < impactCols.Length; i++)
            {
                if (impactCols[i].gameObject.GetComponent<Damage>())
                {
                    impactCols[i].gameObject.GetComponent<Damage>().ApplyDamage(baseDamage * damageMultiplier);
                }
            }
        }
    }

    IEnumerator RocketLife()
    {
        yield return new WaitForSeconds(rocketLife);
        gameObject.SetActive(false);
        isFired = false;
    }
}