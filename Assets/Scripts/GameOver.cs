using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text score;
    public Text time;


    // Start is called before the first frame update
    void Start()
    {
        setRecord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setRecord(){
        GameObject stats = GameObject.Find("Stats");

        this.score.text = "Score = " + stats.GetComponent<Stats>().score.ToString("00000");
        this.time.text = "Time = " + stats.GetComponent<Stats>().time;
    }

    public void menuButtonPressed() {
        SceneManager.LoadScene("Menu");
    }

    public void playButtonPressed() {
        SceneManager.LoadScene("Level");
    }

    public void exitButtonPressed() {
        Application.Quit();
    }
}
