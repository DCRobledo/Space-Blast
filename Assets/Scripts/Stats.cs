using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** STATS *****************/
public class Stats : MonoBehaviour
{
    //Player's score
    public int score;

    //Game's time
    public string time;


    /***************** STARTING METHODS *****************/
    void Awake()
    {
        //We prevent this gameobject from being destroyed when going from the level scene to the game over scene
        DontDestroyOnLoad(this.gameObject);
    }
}
