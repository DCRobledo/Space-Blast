using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** PROJECTILES *****************/
public class Projectile : MonoBehaviour
{
    //Speed management
    public float projectileSpeed;

    //Player's and Enemies' projectiles diferentiation
    public bool isPlayerProjectile = true;

    //Visual Effect
    public GameObject effect;


    /***************** STARTING METHODS *****************/
    void Start()
    {
        projectileEffect();
    }

    private void projectileEffect()
    {
        GameObject newEffect = Instantiate(effect, transform.position, Quaternion.identity);
        newEffect.name = "Projectile Effect";
        newEffect.transform.SetParent(GameObject.Find("Effects").transform);
    }


    /***************** UPDATING METHODS *****************/
    void Update()
    {
        moveProjectile();
    }

    /***************** MOVEMENT *****************/
    private void moveProjectile(){
        Vector3 pos = this.transform.localPosition;

        //We move player's projectiles upwards and enemies' downwards
        if(isPlayerProjectile)
            pos.y += projectileSpeed * Time.deltaTime;
        else
            pos.y -= projectileSpeed * Time.deltaTime;

        this.transform.localPosition = pos;
    }


    /***************** COLLISIONS *****************/
    private void OnCollisionEnter2D(Collision2D collider) {
        if(isPlayerProjectile)
            checkCollisionOnEnemy(collider);
        if(!isPlayerProjectile)
            checkCollisionOnPlayer(collider);

        Destroy(this.gameObject);
    }

    private void checkCollisionOnEnemy(Collision2D collider){
        if(collider.gameObject.tag.Equals("Enemy")){
            collider.transform.GetComponent<Enemy>().enemiesScore();

            collider.transform.GetComponent<Enemy>().explosionEffect();

            GameObject.Find("GameController").GetComponent<GameController>().enemyDown();

            Destroy(collider.gameObject);
        }
    }

    private void checkCollisionOnPlayer(Collision2D collider){
        if(collider.gameObject.tag.Equals("Player"))
            GameObject.Find("UI").GetComponent<UI>().playerHit();
    }
}
