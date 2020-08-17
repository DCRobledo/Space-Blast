using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;

    public float[] xLimits;

    public GameObject enemyProjectile;

    [Range (0, 2)]
    public float shootChance;

    private bool isGoingRight = true;

    private Rigidbody2D rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveShooter();
        shoot();
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
}
