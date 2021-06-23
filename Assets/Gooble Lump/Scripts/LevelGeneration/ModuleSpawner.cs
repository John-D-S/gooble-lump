using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [Header("-- Module Spawning Settings --")]
    [SerializeField, Tooltip("A List of all possible level modules")]
    private List<GameObject> levelModules;
    [SerializeField, Tooltip("The length of the sides of the square within which the module will spawn")]
    public float SpawnSquareSideLength;

    private void Start()
    {
        //Selects a random module from levelModules and spawns it at a random position within a square with sides of length spawnsquaresidelength
        int indexOfModuletoSpawn = Random.Range(0, levelModules.Count);
        float spawnXPos = Random.Range(-SpawnSquareSideLength * 0.5f, SpawnSquareSideLength * 0.5f);
        float spawnYPos = Random.Range(-SpawnSquareSideLength * 0.5f, SpawnSquareSideLength * 0.5f);
        Instantiate(levelModules[indexOfModuletoSpawn], (Vector2)transform.position + new Vector2(spawnXPos, spawnYPos), Quaternion.identity, gameObject.transform);
    }
}
