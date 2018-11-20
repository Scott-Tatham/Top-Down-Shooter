using LazyTitan.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] unitObjs;

    static UnitSpawner unitSpawner;

    void Start()
    {
        if (unitSpawner == null)
        {
            unitSpawner = this;
        }

        else if (unitSpawner != this)
        {
            Destroy(this);
        }
    }

    [DevCommand("Spawn", "Spawns a units of specified amount, using an ID.")]
    public void SpawnUnit(int amount, int id)
    {
        if (id < unitObjs.Length && id >= 0)
        {
            DevConsole.GetDevConsole().NewMessage("Spawning " + amount.ToString().Colour(Color.cyan
                ) + " units of type: " + id.ToString().Colour(Color.blue) + " : " + unitObjs[id].GetComponent<UnitStats>().GetUnitName().Colour(Color.green), false, LogLevel.INFO);

            for (int i = 0; i < amount; i++)
            {
                Vector2 randomPos = Random.insideUnitCircle;
                Instantiate(unitObjs[id], new Vector3(randomPos.x, 1, randomPos.y), Quaternion.identity);
            }
        }

        else
        {
            DevConsole.GetDevConsole().NewMessage("No unit with the ID " + id.ToString().Colour(Color.red) + " exists.", false, LogLevel.ERROR);
        }
    }
}