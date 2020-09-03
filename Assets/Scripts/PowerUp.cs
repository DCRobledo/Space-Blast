using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum powerUpType {
        SHIELD,
        SHOOTBOOST,
        SPEEDBOOST
    }

    public powerUpType type;

    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Player")){
            
            GameObject.Find("GameController").GetComponent<GameController>().powerUpDown();

            explosionEffect();

            switch(type){
                case powerUpType.SHIELD: GameObject.Find("Player").GetComponent<Player>().shield(); break;
                case powerUpType.SHOOTBOOST: GameObject.Find("Player").GetComponent<Player>().shootBoost(); break;
                default: GameObject.Find("Player").GetComponent<Player>().speedBoost(); break;
            }
            
            Destroy(this.gameObject);
        }
    }

    private void explosionEffect(){
        GameObject newObj = Instantiate(effect, transform.position, Quaternion.identity);
        newObj.name = "Explosion Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }

}
