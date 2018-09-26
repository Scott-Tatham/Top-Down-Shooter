using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    float baseMoveSpeed;

    float currentMoveSpeed;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
    }

    public float GetCurrentMoveSpeed() { return currentMoveSpeed; }
}