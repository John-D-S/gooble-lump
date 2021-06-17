using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticObjectHolder;

public class Lava : MonoBehaviour
{
    [SerializeField]
    private float lavaRiseInitialSpeed;
    [SerializeField, Tooltip("how much faster the lava rises per minute")]
    private float lavaRiseSpeedIncrease;
    [SerializeField]
    private float maxDistanceFromPlayer = 100;

    private void FixedUpdate()
    {
        float currentHeight = gameObject.transform.position.y;
        if (player.AveragePosition.y - currentHeight > maxDistanceFromPlayer)
        {
            float targetHeight = Mathf.Lerp(currentHeight, player.AveragePosition.y - maxDistanceFromPlayer + 1, 0.05f);
            gameObject.transform.position = new Vector2(player.AveragePosition.x, targetHeight);
        }
        else
        {
            gameObject.transform.position = new Vector3(player.AveragePosition.x, currentHeight + (lavaRiseInitialSpeed + lavaRiseSpeedIncrease * Time.time * 0.01666f) * Time.fixedDeltaTime);
        }
        DestroyLevelModulesBelowPosition();
    }

    private void DestroyLevelModulesBelowPosition()
    {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            theScoreSystem.SaveScore();
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
