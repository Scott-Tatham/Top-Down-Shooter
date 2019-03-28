using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyTitan.Extensions;

public class EnemyBase : MonoBehaviour, IDamagable<int>
{
    public int startingHealth = 5;
    public int currentHealth;
    public Transform target;
    public float rotSpeed;
    public float moveSpeed;

    bool isInRange;
    bool isHit;
    bool isDead;

    bool isWalking = false;
    bool isWandering = false;
    bool isRotatingRight = false;
    bool isRotatingLeft = false;

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

        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * rotSpeed * Time.deltaTime);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.right * -rotSpeed * Time.deltaTime);
        }

        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    public void Damage(int damageTaken)
    {
        isHit = true;

        if (isDead)
        {
            return;
        }

        currentHealth -= damageTaken;

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

    public IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotWait = Random.Range(1, 4);
        int rotLOrR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 4);
        int walkTime = Random.Range(1, 4);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotWait);

        if (rotLOrR == 1)
        {
            isRotatingRight = true;

            yield return new WaitForSeconds(rotTime);

            isRotatingRight = false;
        }

        if (rotLOrR == 2)
        {
            isRotatingLeft = true;

            yield return new WaitForSeconds(rotTime);

            isRotatingLeft = false;
        }

        isWandering = false;
    }
}
