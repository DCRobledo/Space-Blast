using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/***************** GAME-OVER *****************/
public class GameOver : MonoBehaviour
{
    //Texts
    public Text score;
    public Text time;


    /***************** STARTING METHODS *****************/
    void Start()
    {
        //We need to first enable the transition's image
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        setRecord();

        StartCoroutine(waitForFadeIn());
    }

    private IEnumerator waitForFadeIn() {
        yield return new WaitForSeconds(2f);

        //We hide the transition's image in order to be able to press the buttons
        GameObject.Find("Transition").GetComponent<Image>().enabled = false;
    }


    private void setRecord(){
        GameObject stats = GameObject.Find("Stats");

        this.score.text = "Score = " + stats.GetComponent<Stats>().score.ToString("00000");
        this.time.text = "Time = " + stats.GetComponent<Stats>().time;
    }

    /***************** MAIN MENU BUTTON *****************/
    public void menuButtonPressed() {
        StartCoroutine(processAfterMenuButtonPressed(1.5f));
    }

    private IEnumerator processAfterMenuButtonPressed (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Menu");
    }


    /***************** PLAY BUTTON *****************/
    public void playButtonPressed() {
        StartCoroutine(processAfterPlayButtonPressed(1.5f));
    }

    private IEnumerator processAfterPlayButtonPressed (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Level");
    }

    /***************** EXIT BUTTON *****************/
    public void exitButtonPressed() {
        StartCoroutine(processAfterExitButtonPressed(1.5f));
    }

    private IEnumerator processAfterExitButtonPressed (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        Application.Quit();
    }
}
