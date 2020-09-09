using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum enemyType {
        SHOOTER,
        UFO,
        ROCKET
    }

    public enemyType type;

    [Range (0, 2)]
    public float shootChance;

    public float xSpeed;
    public float ySpeed;

    public GameObject enemyProjectile;
    public GameObject effect;

    public bool canShot;

    private float[] xLimits = {-2, 0};
    private float[] yPeaks;

    private bool isGoingUp = true;
    private bool reachPeak = false;
    private bool isGoingRight = true;

    private Rigidbody2D rb; 


    void Start()
    {
        setRigidBody2D();

        setInitialStats();
    }

    private void setInitialStats()
    {
        isGoingRight = this.transform.localPosition.x < 0;

        float[] UFOPeaks = { this.transform.localPosition.y + .2f, this.transform.localPosition.y - .2f };
        yPeaks = UFOPeaks;
    }

    private void setRigidBody2D()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveEnemy();

        if(canShot)
            shoot();
    }

    private void moveEnemy() {
        switch(type){
            case enemyType.SHOOTER: moveShooter(); break;
            case enemyType.UFO: moveUFO(); break;
            default: moveRocket(); break;
        }
    }

    private void moveShooter(){
        if(isGoingRight) {
            if(this.transform.localPosition.x > xLimits[1])
                isGoingRight = !isGoingRight;
            else
                rb.AddForce(new Vector2(xSpeed, 0));
        }
        else {
            if(this.transform.localPosition.x < xLimits[0])
                isGoingRight = !isGoingRight;
            else
                rb.AddForce(new Vector2(-xSpeed, 0));
        }
    }

    private void moveUFO(){
        Vector3 pos = this.transform.localPosition;

        if(isGoingRight) rb.AddForce(new Vector2(xSpeed*.2f, 0));
        else rb.AddForce(new Vector2(-xSpeed*.2f, 0));

        if(isGoingUp){
            rb.AddForce(new Vector2(0, ySpeed));

            reachPeak = pos.y >= yPeaks[0];
        }
        else{ 
            rb.AddForce(new Vector2(0, -ySpeed));

            reachPeak = pos.y <= yPeaks[1];
        }

        if(reachPeak) isGoingUp = !isGoingUp;
    }

    private void moveRocket(){
        rb.AddForce(new Vector2(0, ySpeed));
    }

    private void shoot() {
        int rnd = Random.Range(0, 500);

        if(rnd < shootChance){
            GameObject projectile = GameObject.Instantiate(enemyProjectile);

            projectile.name = "EnemyProjectile";
            projectile.transform.SetParent(GameObject.Find("EnemyProjectiles").transform);

            Vector3 pos = this.transform.localPosition;
            projectile.transform.localPosition = new Vector3 (pos.x, pos.y - 1.3f, pos.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collider) {
        switch(collider.transform.tag){
            case "Player": collisionWithPlayer(); break;
            case "Enemy": collisionWithEnemy(); break;
            case "Wall": collisionWithWalls(); break;
        }
    }

    private void collisionWithPlayer()
    {
        GameObject.Find("UI").GetComponent<UI>().playerHit();

        enemyDeath();
    }

    private void collisionWithEnemy(){
        if(type == enemyType.ROCKET)
            enemyDeath();
        else
            isGoingRight = !isGoingRight;
    }

    private void collisionWithWalls(){
        if(type == enemyType.SHOOTER)
            isGoingRight = !isGoingRight;
        else
            enemyDeath();
    }

    private void enemyDeath()
    {
        GameObject.Find("GameController").GetComponent<GameController>().enemyDown();

        this.explosionEffect();

        Destroy(this.gameObject);
    }

    public void enemiesScore(){
        int score = 0;

        switch(type){
            case enemyType.UFO: score = 50; break;
            case enemyType.ROCKET: score = 20; break;
            default: score = 75; break;
        }

        GameObject.Find("UI").GetComponent<UI>().addScore(score);
    }

    public void explosionEffect(){
        GameObject newObj = Instantiate(effect, transform.position, Quaternion.identity);
        newObj.name = "Explosion Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }
}
