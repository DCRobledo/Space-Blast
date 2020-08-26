using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public int playerScore = 0;
    public int playerLives = 3;

    public Text score;
    public Text time;
    public Image live0;
    public Image live1;
    public Image live2;

    private float timeCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
        updateLives();
        updateScore();
    }

    private void updateTime() {
        timeCounter += Time.deltaTime;

        string minutes = Mathf.Floor(timeCounter / 60f).ToString("00");
        string seconds = (timeCounter % 60f).ToString("00");

        time.GetComponent<Text>().text = minutes + ":" + seconds;
    }

    private void updateLives() {
        switch(playerLives){
            case 3: setLivesImages(true, true, true); break;

            case 2: setLivesImages(true, true, false); break;

            case 1: setLivesImages(true, false, false); break;

            default: setLivesImages(false, false, false); break;
        }
    }

    private void setLivesImages(bool live0, bool live1, bool live2) {
        this.live0.GetComponent<Image>().enabled = live0;
        this.live1.GetComponent<Image>().enabled = live1;
        this.live2.GetComponent<Image>().enabled = live2;
    }

    private void updateScore() {
        this.score.GetComponent<Text>().text = playerScore.ToString("00000");
    }

    public void playerHit(){
        if(!GameObject.Find("Player").GetComponent<Player>().isRecovering) {
            StartCoroutine(GameObject.Find("Player").GetComponent<Player>().recover());
            if(!GameObject.Find("Player").GetComponent<Player>().shieldUp)
                playerLives--;
            else
                GameObject.Find("Player").GetComponent<Player>().shieldUp = false;

            if(playerLives == 0)
                gameOver();
        }
    }

    private void gameOver(){
        GameObject.Find("Stats").GetComponent<Stats>().score = playerScore;
        GameObject.Find("Stats").GetComponent<Stats>().time = time.GetComponent<Text>().text.ToString();

        SceneManager.LoadScene("GameOver");
    }
}
