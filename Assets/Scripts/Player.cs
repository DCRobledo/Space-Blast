using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    public GameObject playerProjectile;

    public bool isRecovering = false;
    public bool shieldUp = false;


    private Rigidbody2D rb;

    private float shootTimer = 0f;
    private float shootTimeLimit = 6f;
    private float speedBoostTimer = 31f;
    private float speedBoostTimeLimit = 30f;
    private float shootBoostTimer = 21f;
    private float shootBoostTimeLimit = 20f;

    private bool canShot = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        checkCanShoot();
        updatePowerUps();
    }

    private void updatePowerUps(){
        updateShield();
        updateShootBoost();
        updateSpeedBoost();
    }

    private void checkInput(){
        if(Input.GetKey(KeyCode.W))
            movePlayer("y", 1);

        if(Input.GetKey(KeyCode.D))
            movePlayer("x", 1);

        if(Input.GetKey(KeyCode.A))
            movePlayer("x", -1);

        if(Input.GetKey(KeyCode.S))
            movePlayer("y", -1);

        if(Input.GetKeyDown(KeyCode.Space) && canShot){
            shootTimer = 0f;
            shoot(); 
        } 
    }

    private void movePlayer(string direction, int forward){
        if(direction.ToUpper().Equals("X"))
            rb.AddForce(new Vector2(forward*xSpeed, 0));

        if(direction.ToUpper().Equals("Y"))
            rb.AddForce(new Vector2(0, forward*ySpeed));
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
        GameObject projectile = GameObject.Instantiate(playerProjectile);

        projectile.name = "PlayerProjectile";
        projectile.transform.SetParent(GameObject.Find("PlayerProjectiles").transform);

        Vector3 pos = this.transform.localPosition;
        projectile.transform.localPosition = new Vector3 (pos.x, pos.y + .7f, pos.z);
    }

    public IEnumerator recover(){
        isRecovering = true;

        yield return new WaitForSeconds(2f);

        isRecovering = false;
    }

    public void shield(){
        if(!shieldUp)
            shieldUp = true;
    }

    private void updateShield() {
        GameObject.Find("shield").GetComponent<SpriteRenderer>().enabled = shieldUp;
    }

    public void shootBoost() {
        this.shootBoostTimer = 0f;
    }

    private void updateShootBoost(){
        if(shootBoostTimer <= shootBoostTimeLimit){
            shootBoostTimer += .1f * Time.deltaTime;
            shootTimeLimit = 6f;
        }
        else {
            shootTimeLimit = 12f;
        }
    }

    public void speedBoost() {
        this.speedBoostTimer = 0f;
    }
    
    private void updateSpeedBoost(){
        if(speedBoostTimer <= speedBoostTimeLimit){
            speedBoostTimer += .1f * Time.deltaTime;
            xSpeed = 1.2f;
            ySpeed = 1.2f;
        }
        else {
            xSpeed = .8f;
            ySpeed = .8f;
        }
    }
}
