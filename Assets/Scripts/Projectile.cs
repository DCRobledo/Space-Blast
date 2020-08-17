using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;

    public bool isPlayerProjectile = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

        Destroy(this.gameObject);
    }

    private void checkCollisionOnEnemy(Collision2D collider){
        Destroy(collider.gameObject);
    }
}
