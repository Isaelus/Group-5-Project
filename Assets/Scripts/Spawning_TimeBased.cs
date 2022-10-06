using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawning_TimeBased : MonoBehaviour
{
    public int spawnLimit = 20; // Spawn limit will increase over time
    public int startingSpawnSize;
    public float enemySpawnRate = 10;
    public int totalEnemies;
    public float timeSinceLastSpawn = 0;
    public float spawnDelay = 20;
    public GameObject enemyPrefab;
    private const int spawningLocations = 9;
    private  int[] spawningArrayX = {28,-4,46,83,69,21,46,42,79};
    private  int[] spawningArrayY = {-2,-13,-3,-2,-2,-9,-13,4,11};
    private bool[] spawnedHere = {false,false,false,false,false,false,false,false,false};
    private int countedspawns = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiation of variables
        totalEnemies = 0;
        spawnLimit = startingSpawnSize;
    }

    // Update is called once per frame
    void Update()
    {
        //--Caclulate spawn limit-- 
        spawnLimit = spawnLimit + (int)(Mathf.Floor(Time.time / enemySpawnRate));

        //--Try to spawn an enemy--
        if (totalEnemies < spawnLimit){
            Debug.Log(timeSinceLastSpawn);
            Debug.Log(spawnDelay);
            if (timeSinceLastSpawn < spawnDelay){
                

                spawnEnemyRandomly();
            }
        }

    }

    bool spawnEnemyRandomly(){
        Debug.Log("spawnEnemyRandomlyFunction Entered");

        //--Enemy Spawning--
        //Randomly generate an xy cooridinate to check for spawning. Keep generating a potential spawn
        //point until a valid one is found
        int randomLocation;
       if (countedspawns < spawningLocations)
        {
        randomLocation = Random.Range(0, spawningLocations);
        if (spawnedHere[randomLocation] == false) spawnEnemyXY(spawningArrayX[randomLocation], spawningArrayY[randomLocation]);
            totalEnemies++;
            spawnedHere[randomLocation] = true;
        }
        else
        {
            for (int i = 0; i < spawningLocations; i++)
            {
                spawnedHere[i] = false;
            }
        }
            
        //Check a random x y coordinate from spawning array to see if it is a valid point

         
        
        return true;
    }
    void spawnEnemyXY(int x, int y){
        Debug.Log("spawnEnemyFunction Entered");
        GameObject enemyGO = Instantiate<GameObject>(enemyPrefab);
        Vector2 posE = Vector2.zero;
        posE.x = x;
        posE.y = y;
        enemyGO.transform.position = posE;
        timeSinceLastSpawn = Time.time;
    }
}
