﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

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
        checkInput();
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
    }

    private void movePlayer(string direction, int forward){
        if(direction.ToUpper().Equals("X"))
            rb.AddForce(new Vector2(forward*xSpeed, 0));

        if(direction.ToUpper().Equals("Y"))
            rb.AddForce(new Vector2(0, forward*ySpeed));
    }
}
