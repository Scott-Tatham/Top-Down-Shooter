using LazyTitan.Extensions;
using LazyTitan.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField, FieldProperties(RuntimeBehaviour.RUNTIME, true)]
    protected int entityIndex;
    [SerializeField, FieldProperties(RuntimeBehaviour.RUNTIME, true)]
    protected string unitName;

    [Header("Health")]
    [SerializeField, FieldProperties(RuntimeBehaviour.EDITOR, false)]
    protected float intitialHealth;
    [SerializeField, FieldProperties(RuntimeBehaviour.RUNTIME, true)]
    protected float currentHealth;

    [Header("Movement")]
    [SerializeField, FieldProperties(RuntimeBehaviour.EDITOR, false)]
    protected float intitialMoveSpeed;
    [SerializeField, FieldProperties(RuntimeBehaviour.RUNTIME, true)]
    protected float currentMoveSpeed;

    [Header("Effects")]
    [SerializeField, FieldProperties(RuntimeBehaviour.RUNTIME, true)]
    protected bool blind;

    GameObject parent;

    public string GetUnitName() { return unitName; }

    public float GetInitialHealth() { return intitialHealth; }
    public float GetCurrentHealth() { return currentHealth; }

    public float GetCurrentMoveSpeed() { return currentMoveSpeed; }

    public bool GetBlind() { return blind; }

    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
        Death();
    }

    public void SetBlind(bool blind) { this.blind = blind; }

    void Death()
    {
        if (currentHealth <= 0)
        {
            // Return the pool when pooling is created.
            if (parent)
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