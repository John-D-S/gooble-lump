using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        //destroy this if theBackgroundMusic already exists in the scene
        if (theBackgroundMusic != null && theBackgroundMusic != this)
        {
            Destroy(this.gameObject);
            return;
        }
        //set the static theBackgroundMusic variable to this
        theBackgroundMusic = this;
        //set this to not be destroyed when the scene changes so that the music does not stop.
        DontDestroyOnLoad(this);
    }
}
