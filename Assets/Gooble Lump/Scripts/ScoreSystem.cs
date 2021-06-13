using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreDisplay;
    [SerializeField]
    private LineRenderer scoreBar;
    private float playerStartingHeight;
    private float playerMaxHeight;
    private float score;
    private float Score
    {
        get
        {
            return score;
        }
        set
        {
            scoreDisplay.text = Mathf.RoundToInt(value).ToString();
            score = value;
        }
    }

    private void SetBarHeight(float height)
    {
        scoreBar.SetPosition(0, new Vector2(player.AveragePosition.x - 100, height));
        scoreBar.SetPosition(1, new Vector2(player.AveragePosition.x + 100, height));
    }

    private void Start()
    {
        playerStartingHeight = player.AveragePosition.y;
        playerMaxHeight = playerStartingHeight;
    }

    private void FixedUpdate()
    {
        float playerHeight = player.AveragePosition.y;
        if (playerHeight > playerMaxHeight)
        {
            playerMaxHeight = playerHeight;
            Score = playerHeight - playerStartingHeight;
        }
        SetBarHeight(playerMaxHeight);
    }
}
