using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;

/// <summary>
/// holds static objects for other classes to use freely
/// </summary>
public static class StaticObjectHolder
{
    public static SpringyThingyController player;

    public static BackgroundMusic theBackgroundMusic;

    public static SaveLoadSystem theSaveLoadSystem;
    public static ScoreSystem theScoreSystem;

    // variables for destroying modules once they're below the fog
    public static Queue<int> LevelModuleHeights = new Queue<int>();
    public static Dictionary<int, List<GameObject>> LevelModulesByHeight = new Dictionary<int, List<GameObject>>();
}
