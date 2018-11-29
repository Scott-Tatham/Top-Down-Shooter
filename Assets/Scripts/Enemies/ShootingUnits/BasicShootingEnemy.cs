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
    public Vector3 targetPosition;

    float shotCounter;
    float moveSpeedStore;
    Collider col;

    
    LayerMask coverLayer;

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

        //transform.position = Vector3.MoveTowards(transform.position, target.position + diff.normalized * distanceWanted, moveSpeed * Time.deltaTime);

        if (shotCounter >= shotDuration)
        {
            Shoot();
        }

        HandleCover();
    }

    void Shoot()
    {
        shotCounter = 0;

        Debug.Log("Shooting");
    }

    public void HandleCover()
    {
        col = FindClosestCover();

        if (col == null)
        {
            return;
        }

        Vector3 dirToTarget = target.position - col.transform.position;
        dirToTarget.Normalize();

        Vector3 targetPos = col.transform.position + (dirToTarget * -1);
    }

    public Collider FindClosestCover()
    {
        Collider[] coverColls = Physics.OverlapSphere(transform.position, 10, coverLayer);

        float mainDist = float.MaxValue;

        Collider closestCol = null;

        for (int i = 0; i < coverColls.Length; i++)
        {
            float tDist = Vector3.Distance(coverColls[i].transform.position, transform.position);

            if (tDist < mainDist)
            {
                mainDist = tDist;
                closestCol = coverColls[i];
            }
        }

        return closestCol;
    }
}
