using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ammo, IAmmo
{
    [SerializeField, Range(0.01f, 60.0f)]
    float projectileSpeed;
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

        StartCoroutine(ProjectileLife());
    }

    public void WeaponBehaviour()
    {
        if (isFired)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, projectileSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<EnvironmentProperties>() != null)
        {
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