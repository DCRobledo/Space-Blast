using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** POWER-UPS *****************/
public class PowerUp : MonoBehaviour {
    //Power-Up type diferentiation
    public enum powerUpType {
        SHIELD,
        SHOOTBOOST,
        SPEEDBOOST
    }

    public powerUpType type;

    //Visual Effect
    public GameObject effect;

    //Audio Effect
    public AudioClip soundEffect;
    
    /***************** COLLISIONS *****************/
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
            playerPicksUpPowerUp();
        
    }

    /***************** BOOST ACTIVATION *****************/
    private void playerPicksUpPowerUp()
    {
        GameObject.Find("GameController").GetComponent<GameController>().powerUpDown();

        powerUpEffect();

        //We call the corresponding player script's method based on the power-up's type
        switch (type)
        {
            case powerUpType.SHIELD: GameObject.Find("Player").GetComponent<Player>().shield(); break;
            case powerUpType.SHOOTBOOST: GameObject.Find("Player").GetComponent<Player>().shootBoost(); break;
            default: GameObject.Find("Player").GetComponent<Player>().speedBoost(); break;
        }

        Destroy(this.gameObject);
    }

    /***************** VISUAL & AUDIO EFFECTS *****************/
    private void powerUpEffect()
    {
        GameObject.Find("Player").GetComponent<Player>().playSoundEffect(soundEffect);

        GameObject newObj = Instantiate(effect, transform.position, Quaternion.identity);
        newObj.name = "PowerUp Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }
}
