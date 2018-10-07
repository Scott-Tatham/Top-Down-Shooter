using ScottyCode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField]
    float health;
    [SerializeField]
    string unitName;

    int entityIndex;
    GameObject parent;

    public float GetHealth() { return health; }
    public string GetUnitName() { return unitName; }

    public void SetHealth(float health)
    {
        this.health = health;
        Death();
    }

    void Death()
    {
        if (health <= 0)
        {
            // Return the pool when pooling is created.
            if (parent != null)
            {
                gameObject.SetActive(false);
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }
}