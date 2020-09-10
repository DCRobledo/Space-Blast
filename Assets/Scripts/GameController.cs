using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************** GAME CONTROLLER *****************/
public class GameController : MonoBehaviour
{
    //Enemies
    public List<Entity> enemies = new List<Entity>();

    //Power-Ups
    public List<Entity> powerUps = new List<Entity>();

    //Audio effects
    public AudioClip playerExplosionSoundEffect;
    public AudioClip enemyExplosionSoundEffect;

    //Game Status management
    public bool gameOn = false;

    //Entities accounting
    private int numEnemies = 0;
    private int numPowerUps = 0;

    //Spawns management
    private bool canSpawnEnemy = true;
    private bool canSpawnPowerUp = true;

    //Player entity
    private GameObject player;


    /***************** STARTING METHODS *****************/
    void Start()
    {
        player = GameObject.Find("Player");
    }

    /***************** UPDATING METHODS *****************/
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

    /***************** SPAWNS *****************/
    private void spawnEntity(List<Entity> list, bool controller, bool isEnemy){
        //Select the entity
        int entitySelector = Random.Range(0, list.Count);

        //Control the spawnChance
        if(spawnChanceControl(list[entitySelector], controller))
        {
            //Increase number of entities
            increaseNumOfEntities(isEnemy);

            //Instantiate the entity
            GameObject newEntity = instantiateNewEntity(list, isEnemy, entitySelector);

            //Position the entity
            positionNewEntity(list, entitySelector, newEntity);
        }
    }

    private void positionNewEntity(List<Entity> list, int entitySelector, GameObject newEntity)
    {
        int rndX = Random.Range(list[entitySelector].xLimits[0], list[entitySelector].xLimits[1]);
        int rndY = Random.Range(list[entitySelector].yLimits[0], list[entitySelector].yLimits[1]);

        //Prevent instant hit on the player
        float playerX = player.transform.localPosition.x;
        float playerY = player.transform.localPosition.y;

        bool hitOnX = (rndX <= playerX + 1.6f) && (rndX >= playerX - 1.6f);
        bool hitOnY = (rndY <= playerY + 1f) && (rndY >= playerY - 1f);

        //Tweak spawn position if needed
        if (hitOnX && hitOnY)
            newEntity.transform.localPosition = new Vector3(rndX + 1.6f, rndY + 1f, 0);
        else
            newEntity.transform.localPosition = new Vector3(rndX, rndY, 0);
    }

    private GameObject instantiateNewEntity(List<Entity> list, bool isEnemy, int entitySelector)
    {
        GameObject newEntity = GameObject.Instantiate(list[entitySelector].entity);

        if (isEnemy)
        {
            newEntity.name = "Enemy" + (numEnemies - 1);
            newEntity.transform.parent = GameObject.Find("Enemies").transform;
        }
        else
        {
            newEntity.name = "PowerUp" + (numPowerUps - 1);
            newEntity.transform.parent = GameObject.Find("PowerUps").transform;
        }

        return newEntity;
    }

    private void increaseNumOfEntities(bool isEnemy)
    {
        if (isEnemy)
            numEnemies++;
        else
            numPowerUps++;
    }

    private bool spawnChanceControl (Entity entity, bool controller) {
        int rnd = Random.Range(0, 1000);

        //We constrain the spawn to spawnChance/1000 of the time
        return (rnd <= entity.spawnChance && controller);
    }

    /***************** HIT REGISTRATIONS *****************/
    public void enemyDown(){
        numEnemies--;
        playEnemyExplosionSoundEffect();
    }

    public void powerUpDown(){
        StartCoroutine(processAfterPowerUpDown());
    }

    private IEnumerator processAfterPowerUpDown() {
        //We wait until the current power-up's effects pass out before spawing another one
        yield return new WaitForSeconds(10f);

        numPowerUps--;
    }

    /***************** AUDIO EFFECTS *****************/
    private void playEnemyExplosionSoundEffect() {
        this.GetComponent<AudioSource>().PlayOneShot(enemyExplosionSoundEffect);
    }

    public void playPlayerExplosionSoundEffect() {
        this.GetComponent<AudioSource>().PlayOneShot(playerExplosionSoundEffect);
    }
}

/***************** ENTITY CLASS *****************/
[System.Serializable]
public class Entity {
    //Prefab
    public GameObject entity;

    //Spawning limits
    [Range(-10, 5)]
    public int[] xLimits = new int[2];
    [Range(4, -3)]
    public int[] yLimits = new int[2];

    //Spawning chance
    [Range(0, 100)]
    public int spawnChance;
}
