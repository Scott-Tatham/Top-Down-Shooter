using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShootingEnemy : EnemyBase
{
    public float bulletSpeed;
    public float shotDuration;
    public float rotationSpeed;
    public float moveSpeed;
    public float distanceWanted = 20f;

    float shotCounter;
    float moveSpeedStore;

    protected override void Start()
    {
        base.Start();

        moveSpeedStore = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        shotCounter += Time.deltaTime;

        Vector3 diff = transform.position - target.position;
        diff.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, target.position + diff.normalized * distanceWanted, moveSpeed * Time.deltaTime);

        if (shotCounter >= shotDuration)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shotCounter = 0;

        Debug.Log("Shooting");
    }
}
