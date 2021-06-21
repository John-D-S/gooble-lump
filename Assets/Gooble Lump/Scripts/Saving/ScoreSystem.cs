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
    [SerializeField]
    private RectTransform highScoreListContainer;
    [SerializeField]
    private GameObject highScoreListItemPrefab;
    private float playerStartingHeight;
    private float playerMaxHeight;
    
    [SerializeField]
    private TMP_InputField nameInput;
    private string defaultPlayerName = "ANONOMOUS";
    private string PlayerName
    {
        get
        {
            if (nameInput.text.Length < 1)
                return defaultPlayerName;
            else
                return nameInput.text;
        }
        set
        {
            nameInput.text = value;
        }
    }

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

    public void DisplayScore()
    {
        //Debug.Log("DisplayScore() was called");
        theSaveLoadSystem.Load();
        Debug.Log(theSaveLoadSystem.gameData.highScores == null);
        foreach (RectTransform child in highScoreListContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < theSaveLoadSystem.gameData.highScores.Count; i++)
        {
            GameObject newHighScoreListItem = Instantiate(highScoreListItemPrefab, highScoreListContainer);
            string NameToDisplay = theSaveLoadSystem.gameData.highScores[i].name;
            int ScoreToDisplay = theSaveLoadSystem.gameData.highScores[i].score;
            newHighScoreListItem.GetComponent<HighScoreListItem>().SetValues(i + 1, NameToDisplay, ScoreToDisplay);
        }
    }

    public void SaveScore()
    {
        theSaveLoadSystem.Load();
        theSaveLoadSystem.gameData.AddScore(PlayerName, Mathf.RoundToInt(Score));
        theSaveLoadSystem.Save();
        PlayerPrefs.SetString("CurrentPlayerName", PlayerName);
        PlayerPrefs.Save();
    }

    private void InitializeScorekeeping()
    {
        playerStartingHeight = player.AveragePosition.y;
        playerMaxHeight = playerStartingHeight;
        if (PlayerPrefs.HasKey("CurrentPlayerName"))
        {
            PlayerName = PlayerPrefs.GetString("CurrentPlayerName");
        }
    }

    private void UpdateScoreKeeping()
    {
        float playerHeight = player.AveragePosition.y;
        if (playerHeight > playerMaxHeight)
        {
            playerMaxHeight = playerHeight;
            Score = playerHeight - playerStartingHeight;
        }
        SetBarHeight(playerMaxHeight);
    }

    private void OnValidate()
    {
        if (highScoreListItemPrefab)
        {
            if (!highScoreListItemPrefab.GetComponent<HighScoreListItem>())
            {
                highScoreListItemPrefab = null;
            }
        }
    }

    private void Awake()
    {
        theScoreSystem = this;
    }

    private void Start()
    {
        if (gameObject.scene.name == "Main")
        {
            InitializeScorekeeping();
        }
        DisplayScore();
    }

    private void FixedUpdate()
    {
        if (gameObject.scene.name == "Main")
        {
            UpdateScoreKeeping();
        }
    }
}
