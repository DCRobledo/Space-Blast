using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** PLAYER *****************/
public class Player : MonoBehaviour {
    //Speed management
    public float idleSpeed;
    public float boostedSpeed;
    
    //Visual effects
    public GameObject playerProjectile;
    public GameObject shieldEffect;
    public GameObject playerEffect;

    //Animator component
    public Animator animator;

    //Hitting management
    public bool isRecovering = false;
    public bool shieldUp = false;

    //Game Status management
    public bool gameOn = false;

    //Rigidbody2D component
    private Rigidbody2D rb;

    //Player's speed management
    private float playerSpeed;

    //Shooting management
    private float shootTimer = 0f;
    private float shootTimeLimit = 6f;

    //SpeedBoost management
    private float speedBoostTimer = 11f;
    private float speedBoostTimeLimit = 10f;

    //ShootBoost management
    private float shootBoostTimer = 6f;
    private float shootBoostTimeLimit = 5f;

    //Shooting management
    private bool canShot = true;
    

    /***************** STARTING METHODS *****************/
    void Start()
    {
        setRigidBody2D();
        setAnimator();
    }

    private void setAnimator()
    {
        animator = this.GetComponent<Animator>();
    }

    private void setRigidBody2D()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //Prevent the player from rotating on collisions
        rb.freezeRotation = true;
    }


    /***************** UPDATING METHODS *****************/
    void Update() {
        //We only allow movement if the game have already started
        if(gameOn)
            checkInput();

        checkCanShoot();

        updatePowerUps();

        updateAnimations();
    }

    private void updatePowerUps() {
        updateShield();
        updateShootBoost();
        updateSpeedBoost();
    }

    private void updateAnimations() {
        //Animator's state machine's bools management
        animator.SetBool("isRecovery", this.isRecovering);

        //We set the animator's bool based on the management variables
        // timer <= timeLimit -> We are inside the boost effect
        // timer > timeLimit -> We are not inside the boost effect
        animator.SetBool("shootBoost", (shootBoostTimer <= shootBoostTimeLimit));
        animator.SetBool("speedBoost", (speedBoostTimer <= speedBoostTimeLimit));
    }


    /***************** MOVEMENT & SHOOTING *****************/
    private void checkInput()
    {
        checkMovementInput();
        checkShootingInput();
    }

    private void checkMovementInput()
    {
        if (Input.GetKey(KeyCode.W))
            movePlayer("y", 1);

        if (Input.GetKey(KeyCode.D))
            movePlayer("x", 1);

        if (Input.GetKey(KeyCode.A))
            movePlayer("x", -1);

        if (Input.GetKey(KeyCode.S))
            movePlayer("y", -1);
    }

    private void checkShootingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShot)  
            shoot();
    }

    private void movePlayer(string direction, int forward){
        if(direction.ToUpper().Equals("X"))
            rb.AddForce(new Vector2(forward*playerSpeed, 0));

        if(direction.ToUpper().Equals("Y"))
            rb.AddForce(new Vector2(0, forward*playerSpeed));
    }

    private void checkCanShoot(){
        if(shootTimer <= shootTimeLimit){
            canShot = false;
            shootTimer += .1f;
        }
        else
            canShot = true;
    }

    private void shoot() {
        shootTimer = 0f;

        GameObject projectile = GameObject.Instantiate(playerProjectile);

        projectile.name = "PlayerProjectile";
        projectile.transform.SetParent(GameObject.Find("PlayerProjectiles").transform);

        Vector3 pos = this.transform.localPosition;
        projectile.transform.localPosition = new Vector3 (pos.x, pos.y + .7f, pos.z);
    }


    /***************** HIT REGISTRATION *****************/
    public IEnumerator recover() {
        isRecovering = true;

        yield return new WaitForSeconds(2f);

        isRecovering = false;
    }

    public void shield() {
        if(!shieldUp)
            shieldUp = true;
    }


    /***************** POWER-UPS *****************/
    private void updateShield() {
        GameObject.Find("shield").GetComponent<SpriteRenderer>().enabled = shieldUp;
    }

    public void shootBoost() {
        this.shootBoostTimer = 0f;
    }

    private void updateShootBoost(){
        if(shootBoostTimer <= shootBoostTimeLimit){
            shootBoostTimer += 1f * Time.deltaTime;
            shootTimeLimit = 1f;
        }
        else {
            shootTimeLimit = 6f;
        }
    }

    public void speedBoost() {
        this.speedBoostTimer = 0f;
    }
    
    private void updateSpeedBoost(){
        if(speedBoostTimer <= speedBoostTimeLimit){
            speedBoostTimer += 1f * Time.deltaTime;
            playerSpeed = boostedSpeed;
        }
        else
            playerSpeed = idleSpeed;
    }


    /***************** VISUAL EFFECTS *****************/
    public void shieldExplosionEffect(){
        GameObject newObj = Instantiate(shieldEffect, transform.position, Quaternion.identity);
        newObj.name = "Explosion Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }

    public void playerExplosionEffect(){
        GameObject newObj = Instantiate(playerEffect, transform.position, Quaternion.identity);
        newObj.name = "Explosion Effect";
        newObj.transform.SetParent(GameObject.Find("Effects").transform);
    }


    /***************** AUDIO EFFECTS *****************/
    public void playSoundEffect(AudioClip clip) {
        this.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
