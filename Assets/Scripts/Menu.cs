using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Transition").GetComponent<Image>().enabled = true;

        StartCoroutine(waitForFadeIn());
    }

    private IEnumerator waitForFadeIn() {
        yield return new WaitForSeconds(2f);
        GameObject.Find("Transition").GetComponent<Image>().enabled = false;
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
