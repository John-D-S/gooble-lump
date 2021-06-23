using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticObjectHolder;

public class Lava : MonoBehaviour
{
    [Header("-- Lava Rising Settings --")]
    [SerializeField, Tooltip("How fast the lava rises to start with")]
    private float lavaRiseInitialSpeed;
    [SerializeField, Tooltip("How much faster the lava rises per minute")]
    private float lavaRiseSpeedIncrease;
    [SerializeField, Tooltip("If the lava is further than this distance from the player, it will lerp towards it, then resume rising at its normal speed once the distance is les than this distance")]
    private float maxDistanceFromPlayer = 100;

    /// <summary>
    /// This Function checks the last value in the LevelModuleHeights queue and if it is lower than the lava, it destroys the module at that height and removes that height from the LevelModuleHeights Queue and the levelModulesByHeight dictionary
    /// </summary>
    private void DestroyLevelModulesBelowPosition()
    {
        //get the last value in levelModuleHeights, which is usually the lowest. 
        // Even if it isn't the lowest, thats fine because once the height above that is dequeued, that will reveal all the other heights below it one by one. 
        // This will only happen when a module on a row that hasn't been generated yet is generated below the player, before modules that are generated above it.
        int heightToCheck = LevelModuleHeights.Peek();
        if (heightToCheck < transform.position.y)
        {
            if (LevelModulesByHeight.ContainsKey(heightToCheck))
            {
                foreach (GameObject LevelModule in LevelModulesByHeight[heightToCheck])
                    Destroy(LevelModule);
                LevelModulesByHeight.Remove(heightToCheck);
                LevelModuleHeights.Dequeue();
            }
        }
    }

    private void FixedUpdate()
    {
        //save an unaltered copy of the lava's current height
        float currentHeight = gameObject.transform.position.y;
        float targetHeight;
        //if the player further away from the lava than max distance from player, lerp towards y position directly below the player which is maxDistanceFromPlayer - 1 units away
        if (player.AveragePosition.y - currentHeight > maxDistanceFromPlayer)
            targetHeight = Mathf.Lerp(currentHeight, player.AveragePosition.y - maxDistanceFromPlayer + 1, 0.05f);
        //if the distance between the player and the lava is within maxDistanceFromPlayer, steadily raise the lava 
        else
            targetHeight = currentHeight + (lavaRiseInitialSpeed + lavaRiseSpeedIncrease * Time.time * 0.01666f) * Time.fixedDeltaTime;
        //set the x position of the lava to the x position of the player so that it is always below it.
        gameObject.transform.position = new Vector2(player.AveragePosition.x, targetHeight);
        DestroyLevelModulesBelowPosition();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if the player collides with the lava, save the score and reload the scene.
        if (collision.collider.CompareTag("Player"))
        {
            theScoreSystem.SaveScore();
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
