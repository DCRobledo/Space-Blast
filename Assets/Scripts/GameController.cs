using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Entity> enemies = new List<Entity>();
    public List<Entity> powerUps = new List<Entity>();

    public int numEnemies = 0;
    public int numPowerUps = 0;

    private bool canSpawnEnemy = true;
    private bool canSpawnPowerUp = true;

    void Start()
    {
        
    }

    void Update()
    {
        checkSpawns();
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
        int rnd = Random.Range(0, 10000);
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
            float rndX = Random.Range(list[entitySelector].xLimits[0], list[entitySelector].xLimits[1]);
            float rndY = Random.Range(list[entitySelector].yLimits[0], list[entitySelector].yLimits[1]);

            newEntity.transform.localPosition = new Vector3(rndX, rndY, 0);
        }
    }

    public void enemyDown(){
        numEnemies--;
    }

    public IEnumerator powerUpDown(){
        yield return new WaitForSeconds(30f);

        numPowerUps--;
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
