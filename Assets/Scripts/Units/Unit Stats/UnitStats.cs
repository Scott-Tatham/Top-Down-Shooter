using ScottyCode.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    int entityIndex;
    float health;

    void Death()
    {
        if (health <= 0)
        {
            // Return the pool when pooling is created.
            gameObject.SetActive(false);
        }
    }
    
    [DevCommand("LogHealth", "<color=green> Working </color>")]
    static void LogHealth(float health, string word)
    {
        Debug.Log(health + " " + word.Colour(Color.green));
    }
}