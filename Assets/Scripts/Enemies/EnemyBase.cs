using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyTitan.Extensions;

public class EnemyBase : MonoBehaviour
{
    public int startingHealth = 5;
    public int currentHealth;

    bool isHit;
    bool isDead;
    bool isInRange;

    protected virtual void Start()
    {
        currentHealth = startingHealth;
    }

    protected virtual void Update()
    {
        if (isInRange)
        {
            
        }
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
}
