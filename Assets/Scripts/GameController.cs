using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Entity> enemies = new List<Entity>();
    public List<Entity> powerUps = new List<Entity>();

    public AudioClip playerExplosionSoundEffect;
    public AudioClip enemyExplosionSoundEffect;

    public bool gameOn = false;

    private int numEnemies = 0;
    private int numPowerUps = 0;

    private bool canSpawnEnemy = true;
    private bool canSpawnPowerUp = true;

    

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        checkSpawns();

        if(gameOn)
            spawnEntities();
    }

    private void checkSpawns() {
        canSpawnEnemy = numEnemies < 3;
        canSpawnPowerUp = numPowerUps < 1;
    }

    private void spawnEntities(){
        spawnEntity(enemies, canSpawnEnemy, true);
        spawnEntity(powerUps, canSpawnPowerUp, false);
    }

    private void spawnEntity(List<Entity> list, bool controller, bool isEnemy){
        //Select the entity
        int entitySelector = Random.Range(0, list.Count);

        //Control the spawnChance
        int rnd = Random.Range(0, 1000);
        if(rnd <= list[entitySelector].spawnChance && controller){
            //Increase number of entities
            if(isEnemy)
                numEnemies++;
            else
                numPowerUps++;

            //Instantiate the entity
            GameObject newEntity = GameObject.Instantiate(list[entitySelector].entity);

            if(isEnemy){
                newEntity.name = "Enemy" + (numEnemies-1);
                newEntity.transform.parent = GameObject.Find("Enemies").transform;
            }
            else {
                newEntity.name = "PowerUp" + (numPowerUps-1);
                newEntity.transform.parent = GameObject.Find("PowerUps").transform;
            }
            
            //Position the entity
            int rndX = Random.Range(list[entitySelector].xLimits[0], list[entitySelector].xLimits[1]);
            int rndY = Random.Range(list[entitySelector].yLimits[0], list[entitySelector].yLimits[1]);

            //Prevent instant hit on the player
            float playerX = player.transform.localPosition.x;
            float playerY = player.transform.localPosition.y;

            bool hitOnX = (rndX <= playerX + 1.6f) && (rndX >= playerX - 1.6f);
            bool hitOnY = (rndY <= playerY + 1f) && (rndY >= playerY - 1f);

            //Tweak spawn position if needed
            if(hitOnX && hitOnY)
                newEntity.transform.localPosition = new Vector3(rndX+1.6f, rndY+1f, 0);
            else
                newEntity.transform.localPosition = new Vector3(rndX, rndY, 0);
        }
    }

    public void enemyDown(){
        numEnemies--;
        playEnemyExplosionSoundEffect();
    }

    public void powerUpDown(){
        StartCoroutine(processAfterPowerUpDown());
    }

    private IEnumerator processAfterPowerUpDown() {
        yield return new WaitForSeconds(10f);

        numPowerUps--;
    }

    private void playEnemyExplosionSoundEffect() {
        this.GetComponent<AudioSource>().PlayOneShot(enemyExplosionSoundEffect);
    }

    public void playPlayerExplosionSoundEffect() {
        this.GetComponent<AudioSource>().PlayOneShot(playerExplosionSoundEffect);
    }
}

[System.Serializable]
public class Entity {
    public GameObject entity;

    [Range(-10, 5)]
    public int[] xLimits = new int[2];
    [Range(4, -3)]
    public int[] yLimits = new int[2];

    [Range(0, 100)]
    public int spawnChance;
}
