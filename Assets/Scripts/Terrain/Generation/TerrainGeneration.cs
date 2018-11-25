using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A terrain chunk data container.
/// </summary>
public class TerrainLayout
{
    bool[,] tileStates;

    public bool GetTile(Vector2Int position) { return tileStates[position.x, position.y]; }
    public int GetTilesX() { return tileStates.GetLength(0); }
    public int GetTilesZ() { return tileStates.GetLength(1); }

    public TerrainLayout(bool[,] tileStates)
    {
        this.tileStates = tileStates;
    }
}

/// <summary>
/// Custom inspector for the terrain generator.
/// </summary>
[CustomPropertyDrawer(typeof(TerrainGeneration))]
public class TerrainGenerationDrawer : Editor
{
    public override void OnInspectorGUI()
    {


    }
}

/// <summary>
/// Terrain generator.
/// </summary>
public class TerrainGeneration : MonoBehaviour
{
    [SerializeField]
    int cycles;
    [SerializeField]
    Vector2Int gridSize;
    [SerializeField]
    Material altTerrain;
    [SerializeField]
    GameObject tileObj;
    [SerializeField]
    bool[] rules;

    static TerrainGeneration terrainGeneration;

    List<TerrainLayout> terrainLayouts;

    void Start()
    {
        if (!terrainGeneration)
        {
            terrainGeneration = this;
        }

        else if (terrainGeneration != this)
        {
            Destroy(this);
        }

        terrainLayouts = new List<TerrainLayout>();

        GenerateNewLayout(cycles, gridSize);
        BuildLayout(0);
    }

    public void GenerateNewLayout(int cycles, Vector2Int gridSize)
    {
        bool[,] tileStates = new bool[gridSize.x, gridSize.y];
        bool[,] stateMask = new bool[gridSize.x, gridSize.y];

        for (int i = 0; i < tileStates.GetLength(0); i++)
        {
            for (int j = 0; j < tileStates.GetLength(1); j++)
            {
                tileStates[i, j] = (Random.value > 0.5f);
            }
        }

        for (int i = 0; i < cycles; i++)
        {
            for (int j = 0; j < tileStates.GetLength(0); j++)
            {
                for (int k = 0; k < tileStates.GetLength(1); k++)
                {
                    stateMask[j, k] = NeighbourCheck(new Vector2Int(j, k), tileStates);
                }
            }

            tileStates = stateMask;
        }

        terrainLayouts.Add(new TerrainLayout(tileStates));
    }

    bool NeighbourCheck(Vector2Int position, bool[,] tileStates)
    {
        int aliveNeighbours = 0;

        for (int i = -1; i < 2; i++)
        {
            if (position.x + i >= 0 && position.x + i < tileStates.GetLength(0))
            {
                for (int j = -1; j < 2; j++)
                {
                    if (position.y + j >= 0 && position.y + j < tileStates.GetLength(1))
                    {
                        if (i == 0 && j == 0)
                        {
                            break;
                        }

                        if (tileStates[position.x + i, position.y + j])
                        {
                            aliveNeighbours++;
                        }
                    }
                }
            }
        }

        return RuleCheck(aliveNeighbours);
    }

    bool RuleCheck(int aliveNeighbours)
    {
        switch (aliveNeighbours)
        {
            case 0:

                return rules[0];

            case 1:

                return rules[1];

            case 2:

                return rules[2];

            case 3:

                return rules[3];

            case 4:

                return rules[4];

            case 5:

                return rules[5];

            case 6:

                return rules[6];

            case 7:

                return rules[7];

            default:
                Debug.LogWarning("Rule out of range. Defaulting to false.");

                return false;
        }
    }

    void BuildLayout(int index)
    {
        for (int i = 0; i < terrainLayouts[index].GetTilesX(); i++)
        {
            for (int j = 0; j < terrainLayouts[index].GetTilesZ(); j++)
            {
                int yOffset = terrainLayouts[index].GetTile(new Vector2Int(i, j)) ? 1 : 0;
                GameObject go = Instantiate(tileObj, new Vector3(i, yOffset, j), Quaternion.identity);
                go.GetComponent<Renderer>().material = yOffset == 0 ? go.GetComponent<Renderer>().material : altTerrain;
            }
        }
    }
}