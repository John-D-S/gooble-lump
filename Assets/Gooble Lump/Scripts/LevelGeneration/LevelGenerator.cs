using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("The ")]
    private float levelGenerationAngle = 45f;
    [SerializeField, Tooltip("The radius around the player where the level will generate")]
    private float spawnerSpawningRadius = 35f;
    [SerializeField]
    private GameObject moduleSpawnerObject;
    [SerializeField]
    float moduleSpawnerGridDistance = 10;
    
    [SerializeField, HideInInspector]
    private ModuleSpawner moduleSpawnerScript;
    [SerializeField, HideInInspector]
    private Vector2 levelGenAnglePos;

    private Dictionary<Vector2, GameObject> moduleSpawners = new Dictionary<Vector2, GameObject>();

    private Vector3 NearestGridPosition(Vector2 _position)
    {
        Vector2 nearestGridPosition = new Vector2(_position.x - _position.x % moduleSpawnerGridDistance, _position.y - _position.y % moduleSpawnerGridDistance);
        return nearestGridPosition;
    }

    private bool WithinLevelGenerationArea(Vector2 _position)
    {
        return Vector2.Dot(Vector2.up, _position.normalized) > Vector2.Dot(Vector2.up, levelGenAnglePos);
    }

    private void TrySpawnModuleAtPosition(Vector2 _position)
    {
        if (!moduleSpawners.ContainsKey(_position) && WithinLevelGenerationArea(_position))
        {
            moduleSpawners[_position] = Instantiate(moduleSpawnerObject, _position, Quaternion.identity);
        }
    }

    private List<Vector2> ModuleSpawnerSpawnPointsAroundPlayer()
    {
        List<Vector2> pointsToReturn = new List<Vector2>();
        int numberOfIterations = Mathf.RoundToInt((spawnerSpawningRadius - spawnerSpawningRadius % moduleSpawnerGridDistance) * 2 / moduleSpawnerGridDistance);
        for (int y = 0; y < numberOfIterations; y++)
        {
            for (int x = 0; x < numberOfIterations; x++)
            {
                float xSpawnPos = (x - numberOfIterations * 0.5f) * moduleSpawnerGridDistance;
                float ySpawnPos = (y - numberOfIterations * 0.5f) * moduleSpawnerGridDistance;
                pointsToReturn.Add(new Vector2(xSpawnPos, ySpawnPos) + player.AveragePosition);
            }
        }
        return pointsToReturn;
    }

    private void OnValidate()
    {
        levelGenAnglePos = new Vector2(Mathf.Sin(Mathf.Deg2Rad * levelGenerationAngle * 0.5f), Mathf.Cos(Mathf.Deg2Rad * levelGenerationAngle * 0.5f));
        if (moduleSpawnerObject)
        {
            moduleSpawnerScript = moduleSpawnerObject.GetComponent<ModuleSpawner>();
        }
    }

    private void FixedUpdate()
    {
        foreach (Vector2 point in ModuleSpawnerSpawnPointsAroundPlayer())
        {
            //Debug.Log("iteration");
            TrySpawnModuleAtPosition(NearestGridPosition(point));
        }
    }
}
