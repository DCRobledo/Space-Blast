using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    public GameObject playerProjectile;

    public bool isRecovering = false;

    private Rigidbody2D rb;

    private float shootTimer = 0f;
    private float shootTimeLimit = 3f;

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
}
