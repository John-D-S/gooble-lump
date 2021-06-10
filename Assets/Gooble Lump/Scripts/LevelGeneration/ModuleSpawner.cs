using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> levelModules;
    [SerializeField]
    public float SpawnSquareSideLength;

    private void Start()
    {
        int indexOfModuletoSpawn = Random.Range(0, levelModules.Count);
        float spawnXPos = Random.Range(-SpawnSquareSideLength * 0.5f, SpawnSquareSideLength * 0.5f);
        float spawnYPos = Random.Range(-SpawnSquareSideLength * 0.5f, SpawnSquareSideLength * 0.5f);
        Instantiate(levelModules[indexOfModuletoSpawn], (Vector2)transform.position + new Vector2(spawnXPos, spawnYPos), Quaternion.identity, gameObject.transform);
    }
}
