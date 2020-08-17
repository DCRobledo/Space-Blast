using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;

    public float[] xLimits;

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
}
