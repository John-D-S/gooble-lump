using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        if (theBackgroundMusic != null && theBackgroundMusic != this)
        {
            Destroy(this.gameObject);
            return;
        }
        theBackgroundMusic = this;
        DontDestroyOnLoad(this);
    }
}
