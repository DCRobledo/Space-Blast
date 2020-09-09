using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {
    public Text score;
    public Text time;
    public Text ready;
    public Text go;
    public Text gameOverText;

    public Image live0;
    public Image live1;
    public Image live2;

    public AudioClip shieldDownSoundEffect;
    public AudioClip playerHitSoundEffect;

    private float timeCounter = 0f;

    private int playerScore = 0;
    private int playerLives = 3;

    private bool gameOn = false;

    void Start()
    {
        startGame();
    }

    void Update()
    {
        if(gameOn)
            updateTime();

        updateLives();
        updateScore();

    }

    private void startGame() {
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        ready.GetComponent<Text>().enabled = false;
        go.GetComponent<Text>().enabled = false;

        StartCoroutine(waitForFadeIn(2f));
    }

    private IEnumerator waitForFadeIn(float delay) {
        yield return new WaitForSeconds(delay);

        GameObject.Find("Transition").GetComponent<Image>().enabled = false;

        StartCoroutine(readyRoutine(2f));
    }

    private IEnumerator readyRoutine(float delay){
        ready.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(delay);

        ready.GetComponent<Text>().enabled = false;

        StartCoroutine(goRoutine(1.5f));
    }

    private IEnumerator goRoutine(float delay){
        go.GetComponent<Text>().enabled = true;

        changeGameStatus(true);

        yield return new WaitForSeconds(delay);

        go.GetComponent<Text>().enabled = false;
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
        if(!GameObject.Find("Player").GetComponent<Player>().isRecovering)
        {
            GameObject.Find("Main Camera").GetComponent<Animator>().SetTrigger("cameraShake");

            StartCoroutine(GameObject.Find("Player").GetComponent<Player>().recover());

            shieldAndLivesManagement();

            checkGameOver();
        }
    }

    private void checkGameOver()
    {
        if (playerLives == 0)
        {
            GameObject.Find("Player").GetComponent<Player>().playerExplosionEffect();
            GameObject.Find("GameController").GetComponent<GameController>().playPlayerExplosionSoundEffect();
            gameOver();
        }
    }

    private void shieldAndLivesManagement()
    {
        if (!GameObject.Find("Player").GetComponent<Player>().shieldUp)
        {
            playerLives--;
            GameObject.Find("Player").GetComponent<Player>().playSoundEffect(playerHitSoundEffect);
        }
        else
        {
            GameObject.Find("Player").GetComponent<Player>().shieldUp = false;
            GameObject.Find("Player").GetComponent<Player>().shieldExplosionEffect();
            GameObject.Find("Player").GetComponent<Player>().playSoundEffect(shieldDownSoundEffect);
        }
    }

    private void gameOver()
    {
        //Stop the game
        changeGameStatus(false);

        //Hide the player
        hidePlayer();

        //Pause Music
        GameObject.Find("Level").GetComponent<AudioSource>().Stop();

        //Destroy Entities
        destroyEntities();

        //Pass Stats
        passStats();

        StartCoroutine(gameOverTextRoutine(1.5f));
    }

    private static void hidePlayer()
    {
        GameObject.Find("Player").GetComponent<Animator>().enabled = false;
        GameObject.Find("Player").GetComponent<SpriteRenderer>().enabled = false;
    }

    private void changeGameStatus(bool status)
    {
        this.gameOn = status;
        GameObject.Find("Player").GetComponent<Player>().gameOn = status;
        GameObject.Find("GameController").GetComponent<GameController>().gameOn = status;
    }

    private IEnumerator gameOverTextRoutine(float delay){
        yield return new WaitForSeconds(1f);

        gameOverText.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(delay);

        gameOverText.GetComponent<Text>().enabled = false;

        StartCoroutine(processAfterGameOver(1.5f));
    }

    private void destroyEntities(){
        destroyEntitiesWithTag("Enemy");
        destroyEntitiesWithTag("PowerUp");
        destroyEntitiesWithTag("PlayerProjectile");
        destroyEntitiesWithTag("EnemyProjectile");
    }

    private void destroyEntitiesWithTag(string tag){
        GameObject[] entities = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject go in entities)
            Destroy(go);
    }

    private void passStats() {
        GameObject.Find("Stats").GetComponent<Stats>().score = playerScore;
        GameObject.Find("Stats").GetComponent<Stats>().time = time.GetComponent<Text>().text.ToString();
    }

    private IEnumerator processAfterGameOver (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameOver");
    }

    public void addScore(int score) {
        playerScore += score;
    }
}
