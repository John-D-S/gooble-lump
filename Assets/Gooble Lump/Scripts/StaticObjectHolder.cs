using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticObjectHolder
{
    public static SpringyThingyController player;

    public static Queue<int> LevelModuleHeights = new Queue<int>();
    public static Dictionary<int, List<GameObject>> LevelModulesByHeight = new Dictionary<int, List<GameObject>>();
}
