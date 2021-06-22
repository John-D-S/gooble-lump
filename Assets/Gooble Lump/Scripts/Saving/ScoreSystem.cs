using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;
using TMPro;

/// <summary>
/// The thing that contains, calculates, saves and displays the players' scores.
/// </summary>
public class ScoreSystem : MonoBehaviour
{
    [Header("-- Display Objects --")]
    [SerializeField, Tooltip("The text that displays the current player's score")]
    private TextMeshProUGUI scoreDisplay;
    [SerializeField, Tooltip("The line renderer in the game bar that raises with the current score")]
    private LineRenderer scoreBar;
    [SerializeField, Tooltip("The container that holds all the highscore records.")]
    private RectTransform highScoreListContainer;
    [SerializeField, Tooltip("The UI prefab that holds the records of the player's highscore. It has to have the highscorelistitem component.")]
    private GameObject highScoreListItemPrefab;
    private float playerStartingHeight;
    private float playerMaxHeight;
    
    [SerializeField, Tooltip("The UI input field where the player inputs their name.")]
    private TMP_InputField nameInput;
    private string defaultPlayerName = "ANONOMOUS";
    /// <summary>
    /// this property comes takes the name of the player from the nameInput input field
    /// </summary>
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
    /// <summary>
    /// setting this sets the score
    /// </summary>
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

    /// <summary>
    /// this sets the height of the score line renderer
    /// </summary>
    /// <param name="height">the height to set the score bar</param>
    private void SetBarHeight(float height)
    {
        //the x position of both linerenderer positions is added to the player position so that it always under the player
        scoreBar.SetPosition(0, new Vector2(player.AveragePosition.x - 100, height));
        scoreBar.SetPosition(1, new Vector2(player.AveragePosition.x + 100, height));
    }

    /// <summary>
    /// This puts up all the saved high scores on 
    /// </summary>
    public void DisplayScore()
    {
        //load the save data, destroy each of the current top highscores and add them back
        theSaveLoadSystem.Load();
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

    /// <summary>
    /// this saves the current score to the file. and sets the playername in the playerprefs.
    /// </summary>
    public void SaveScore()
    {
        theSaveLoadSystem.Load();
        theSaveLoadSystem.gameData.AddScore(PlayerName, Mathf.RoundToInt(Score));
        theSaveLoadSystem.Save();
        PlayerPrefs.SetString("CurrentPlayerName", PlayerName);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// sets the starting height to be the players y position and sets the player name in the player name input field from playerprefs 
    /// </summary>
    private void InitializeScorekeeping()
    {
        //set the player starting height to theplayer's Average height.
        playerStartingHeight = player.AveragePosition.y;
        playerMaxHeight = playerStartingHeight;
        //set the player name in the player name input field from playerprefs 
        if (PlayerPrefs.HasKey("CurrentPlayerName"))
        {
            PlayerName = PlayerPrefs.GetString("CurrentPlayerName");
        }
    }

    /// <summary>
    /// checks if the player's height is greater than the player's max height
    /// </summary>
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
        //check if the highScoreListItemPrefab has the HighScoreListItem component
        if (highScoreListItemPrefab)
            if (!highScoreListItemPrefab.GetComponent<HighScoreListItem>())
                highScoreListItemPrefab = null;
    }

    private void Awake()
    {
        //set the static score system object to this
        theScoreSystem = this;
    }

    private void Start()
    {
        if (gameObject.scene.name == "Main")
            InitializeScorekeeping();
        DisplayScore();
    }

    private void FixedUpdate()
    {
        if (gameObject.scene.name == "Main")
            UpdateScoreKeeping();
    }
}
