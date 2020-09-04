using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text score;
    public Text time;

    void Start()
    {
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        setRecord();

        StartCoroutine(waitForFadeIn());
    }

    private IEnumerator waitForFadeIn() {
        yield return new WaitForSeconds(2f);
        GameObject.Find("Transition").GetComponent<Image>().enabled = false;
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
        StartCoroutine(processAfterMenuButtonPressed(1.5f));
    }

    private IEnumerator processAfterMenuButtonPressed (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Menu");
    }

    public void playButtonPressed() {
        StartCoroutine(processAfterPlayButtonPressed(1.5f));
    }

    private IEnumerator processAfterPlayButtonPressed (float delay){
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Level");
    }

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
