using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;

    public bool isPlayerProjectile = true;

    public GameObject effect;


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

    void Update()
    {
        moveProjectile();
    }

    private void moveProjectile(){
        Vector3 pos = this.transform.localPosition;

        if(isPlayerProjectile)
            pos.y += projectileSpeed * Time.deltaTime;
        else
            pos.y -= projectileSpeed * Time.deltaTime;

        this.transform.localPosition = pos;
    }

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
