using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyTitan.Extensions;

public class EnemyBase : MonoBehaviour
{
    public int startingHealth = 5;
    public int currentHealth;
    public Transform target;

    bool isInRange;
    bool isHit;
    bool isDead;

    int playerLayer;

    protected virtual void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        currentHealth = startingHealth;
    }

    protected virtual void Update()
    {
        if (isInRange)
        {
            Debug.Log("Attack");
            Attack();
        }

        isHit = false;
        Turning();
    }

    public void TakeDamage(int amount)
    {
        isHit = true;

        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            isInRange = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            isInRange = false;
        }
    }

    void Attack()
    {
        Debug.Log("Player hit");
    }

    void Turning()
    {
        transform.LookAt(target);
    }
}
