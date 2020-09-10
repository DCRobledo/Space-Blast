using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** ENEMIES *****************/
public class Enemy : MonoBehaviour {
    //Enemy type diferentiation
    public enum enemyType {
        SHOOTER,
        UFO,
        ROCKET
    }

    public enemyType type;

    //Shooting chance
    [Range (0, 2)]
    public float shootChance;

    //Speed management
    public float xSpeed;
    public float ySpeed;

    //Shooter's projectile
    public GameObject enemyProjectile;
    public GameObject effect;

    //Shooting management
    public bool canShot;

    //Shooter's and UFO's movement limits
    private float[] xLimits = {-2, 0};
    private float[] yPeaks;

    //Shooter's and UFO's movement management
    private bool isGoingUp = true;
    private bool reachPeak = false;
    private bool isGoingRight = true;

    //Rigidbody2D component
    private Rigidbody2D rb; 


    /***************** STARTING METHODS *****************/
    void Start()
    {
        setRigidBody2D();
        setInitialStats();
    }

    private void setInitialStats()
    {
        //Enemies starting on the left side go to the right, and viceversa
        isGoingRight = this.transform.localPosition.x < 0;

        //We set the UFO peaks based on its spawing position
        float[] UFOPeaks = { this.transform.localPosition.y + .2f, this.transform.localPosition.y - .2f };
        yPeaks = UFOPeaks;
    }

    private void setRigidBody2D()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //Prevent enemies from rotating on collisions
        rb.freezeRotation = true;
    }


    /***************** UPDATING METHODS *****************/
    void Update()
    {
        moveEnemy();

        if(canShot)
            shoot();
    }


    /***************** MOVEMENT *****************/
    private void moveEnemy() {
        //We call the corresponding movement path based on the enemy's type
        switch(type){
            case enemyType.SHOOTER: moveShooter(); break;
            case enemyType.UFO: moveUFO(); break;
            default: moveRocket(); break;
        }
    }

    private void moveShooter(){
        //Simple X-Axis movement
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
        //Wave X-And-Y-Axis movement
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
        //Simple Y-Axis movement
        rb.AddForce(new Vector2(0, ySpeed));
    }


    /***************** SHOOTING *****************/
    private void shoot() {
        int rnd = Random.Range(0, 500);

        //We constraing the shooting to shootingChance/500 of the time
        if(rnd < shootChance){
            GameObject projectile = GameObject.Instantiate(enemyProjectile);

            projectile.name = "EnemyProjectile";
            projectile.transform.SetParent(GameObject.Find("EnemyProjectiles").transform);

            Vector3 pos = this.transform.localPosition;
            projectile.transform.localPosition = new Vector3 (pos.x, pos.y - 1.3f, pos.z);
        }
    }


    /***************** COLLISIONS *****************/
    private void OnCollisionEnter2D(Collision2D collider) {
        //We call the corresponding collision method based on the enemy's type
        switch(collider.transform.tag){
            case "Player": collisionWithPlayer(); break;
            case "Enemy": collisionWithEnemy(); break;
            case "Wall": collisionWithWalls(); break;
        }
    }

    private void collisionWithPlayer()
    {
        //Every enemy gets destroyed and hurts the players when colliding with them
        GameObject.Find("UI").GetComponent<UI>().playerHit();
        enemyDeath();
    }

    private void collisionWithEnemy(){
        //Only Rockets get destroyed when hitting another enemy
        if(type == enemyType.ROCKET)
            enemyDeath();
        else
            isGoingRight = !isGoingRight;
    }

    private void collisionWithWalls(){
        //Only Shooters don't get destroyed when hitting walls
        if(type == enemyType.SHOOTER)
            isGoingRight = !isGoingRight;
        else
            enemyDeath();
    }

    /***************** HIT REGISTRATION *****************/
    private void enemyDeath()
    {
        GameObject.Find("GameController").GetComponent<GameController>().enemyDown();

        this.explosionEffect();

        Destroy(this.gameObject);
    }

    /***************** SCORE *****************/
    public void enemiesScore(){
        int score = 0;

        //We set the score based on the enemy's type
        switch(type){
            case enemyType.UFO: score = 50; break;
            case enemyType.ROCKET: score = 20; break;
            default: score = 75; break;
        }

        GameObject.Find("UI").GetComponent<UI>().addScore(score);
    }

    /***************** VISUAL EFFECTS *****************/
    public void explosionEffect(){
        GameObject newObj = Instantiate(effect, transform.position, Quaternion.identity);
        newObj.name = "Explosion Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }
}
