using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyTitan.Extensions;

public class ChargerEnemies : EnemyBase
{
    public float moveSpeed;
    public float timeBetweenAttack = 2;

    public ChargerStates chargerState;
    
    float distance;
    float moveSpeedStore;
    float timer;
    float time = 2;
    float sphereTimer;

    bool startTimer;
    
    SphereCollider dmgCollider;

    private void Awake()
    {
        dmgCollider = GetComponent<SphereCollider>();

        moveSpeedStore = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        ChargerStates();
        timer += Time.deltaTime;
    }

    void ChargerStates()
    {
        distance = (transform.position - target.transform.position).sqrMagnitude;

        switch (chargerState)
        {
            case global::ChargerStates.Charge:

                if (distance < 2500)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
                }

                if (distance < 1)
                {
                    Debug.Log("Is in range");
                    chargerState = global::ChargerStates.Stop;
                }
                break;

            case global::ChargerStates.Stop:

                moveSpeed = 0;

                if (timer >= timeBetweenAttack)
                {
                    timer = 0;
                    moveSpeed = moveSpeedStore;
                    chargerState = global::ChargerStates.Explode;
                }
                break;

            case global::ChargerStates.Explode:

                if (Time.time >= time)
                {
                    if (dmgCollider.radius < 10)
                    {
                        dmgCollider.radius += 1f;
                    }
                    else
                    {
                        dmgCollider.enabled = false;
                    }
                }
                break;

        }
    }
}

public enum ChargerStates
{
    Charge,
    Stop, 
    Explode
}
