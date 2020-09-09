using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public enum powerUpType {
        SHIELD,
        SHOOTBOOST,
        SPEEDBOOST
    }

    public powerUpType type;

    public GameObject effect;

    public AudioClip soundEffect;
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
            playerPicksUpPowerUp();
        
    }

    private void playerPicksUpPowerUp()
    {
        GameObject.Find("GameController").GetComponent<GameController>().powerUpDown();

        powerUpEffect();

        switch (type)
        {
            case powerUpType.SHIELD: GameObject.Find("Player").GetComponent<Player>().shield(); break;
            case powerUpType.SHOOTBOOST: GameObject.Find("Player").GetComponent<Player>().shootBoost(); break;
            default: GameObject.Find("Player").GetComponent<Player>().speedBoost(); break;
        }

        Destroy(this.gameObject);
    }

    private void powerUpEffect()
    {
        GameObject.Find("Player").GetComponent<Player>().playSoundEffect(soundEffect);

        GameObject newObj = Instantiate(effect, transform.position, Quaternion.identity);
        newObj.name = "PowerUp Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }
}
