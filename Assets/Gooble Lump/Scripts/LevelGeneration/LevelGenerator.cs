using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;

public class LevelGenerator : MonoBehaviour
{
    [Header("-- Level Generation Settings --")]
    [SerializeField, Tooltip("The angle which determines the area above the origin in which the level can generate. An angle less than 180 will generate the level within a V shape above the origin.")]
    private float levelGenerationAngle = 45f;
    [SerializeField, Tooltip("The Area around the player starting position at which level modules will not spawn.")]
    private float startClearRadius;
    [SerializeField, Tooltip("The radius around the spawner at which the actual objects will generate.")]
    private float spawnerSpawningRadius = 50f;
    [SerializeField, Tooltip("The radius around the player where the level will generate.")]
    private float playerSpawnAreaRadius = 50f;
    [SerializeField, Tooltip("The object that is spawned by this level generator script.")]
    private GameObject moduleSpawnerObject;
    [SerializeField, Tooltip("The distance between each grid position.")]
    float moduleSpawnerGridDistance = 10;
    
    [SerializeField, HideInInspector]
    private ModuleSpawner moduleSpawnerScript;
    //used to find out whether a position is within an the allowed angle
    [SerializeField, HideInInspector]
    private Vector2 levelGenAnglePos;

    //a dictionary of already spawned modules. used for finding out if a module already exists at a position.
    private Dictionary<Vector2, GameObject> moduleSpawners = new Dictionary<Vector2, GameObject>();

    /// <summary>
    /// Returns the nearest position on the grid to _position;
    /// </summary>
    private Vector3 NearestGridPosition(Vector2 _position)
    {
        Vector2 nearestGridPosition = new Vector2(_position.x - _position.x % moduleSpawnerGridDistance, _position.y - _position.y % moduleSpawnerGridDistance);
        return nearestGridPosition;
    }

    /// <summary>
    /// Returns true if _position is within the level generation area as is determined by levelGenerationAngle
    /// </summary>
    private bool WithinLevelGenerationArea(Vector2 _position)
    {
        return Vector2.Dot(Vector2.up, _position.normalized) > Vector2.Dot(Vector2.up, levelGenAnglePos);
    }

    /// <summary>
    /// Instantiate the module if possible.
    /// </summary>
    private void TrySpawnModuleAtPosition(Vector2 _position)
    {
        //if there is not already a module at the position _position and _position is within the levelgenerationArea, instantiate the module.
        if (!moduleSpawners.ContainsKey(_position) && WithinLevelGenerationArea(_position))
        {
            //instantiate the module
            moduleSpawners[_position] = Instantiate(moduleSpawnerObject, _position, Quaternion.identity);
            //add a record of the module and/or its position to all the lists and dictionaries and queues that they need to be in.
            int yHeightInt = Mathf.RoundToInt(_position.y);
            if (!LevelModulesByHeight.ContainsKey(yHeightInt))
            {
                LevelModulesByHeight[yHeightInt] = new List<GameObject>();
                LevelModuleHeights.Enqueue(yHeightInt);
            }
            LevelModulesByHeight[yHeightInt].Add(moduleSpawners[_position]);
        }
    }

    /// <summary>
    /// Returns a list of all the positions on the grid around the player.
    /// </summary>
    private List<Vector2> ModuleSpawnerSpawnPointsAroundPlayer(float radius)
    {
        // setting the variable which will be returned to a new empty list
        List<Vector2> pointsToReturn = new List<Vector2>();
        int numberOfIterations = Mathf.RoundToInt((radius - radius % moduleSpawnerGridDistance) * 2 / moduleSpawnerGridDistance);
        //iterate over all the points in a square around the player, separated by gridlength and add them to the return value 
        for (int y = 0; y < numberOfIterations; y++)
        {
            for (int x = 0; x < numberOfIterations; x++)
            {
                float xSpawnPos = (x - numberOfIterations * 0.5f) * moduleSpawnerGridDistance;
                float ySpawnPos = (y - numberOfIterations * 0.5f) * moduleSpawnerGridDistance;
                pointsToReturn.Add(new Vector2(xSpawnPos, ySpawnPos) + player.AveragePosition);
            }
        }
        // return the return value
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

    private void Start()
    {
        //set all the points around the player to be null at start so that no modules can spawn there and the player has room to move.
        foreach (Vector2 point in ModuleSpawnerSpawnPointsAroundPlayer(playerSpawnAreaRadius))
        {
            moduleSpawners[NearestGridPosition(point)] = null;
        }
    }

    private void FixedUpdate()
    {
        foreach (Vector2 point in ModuleSpawnerSpawnPointsAroundPlayer(spawnerSpawningRadius))
        {
            TrySpawnModuleAtPosition(NearestGridPosition(point));
        }
    }
}
