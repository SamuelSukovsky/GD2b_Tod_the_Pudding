using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]                              // Spawn settings and variables
    [SerializeField] private float spawnRate = 10f;
    [SerializeField] private int points = 2;
    [SerializeField] private int pointIncrease = 3;
    [SerializeField] private float spawnRadius;
    [SerializeField] private Vector2 posArenaCorner;
    [SerializeField] private Vector2 negArenaCorner;
    private float countdown;
    private int currentPoints;
    private int increasePoints = 0;
    public GameObject player;
    public GameObject[] enemies;                            // And array for enemy prefabs

    void Awake()                                            // Before first frame
    {
        countdown = spawnRate;                                  // Set countdown for first wave
    }
    
    void Update()                                           // Each frame
    {
        countdown -= Time.deltaTime;                            // Count down
        if(countdown <= 0f)                                     // If countdown is zero
        {
            SpawnWave();                                            // Spawn a new wave
        }
    }

    void SpawnWave()                                        // Spawn wave
    {
        increasePoints++;                                       // Add progress towards increasing wave size
        if(increasePoints == pointIncrease)                     // If point increase reached 
        {
            increasePoints = 0;                                     // Reset progress
            points++;                                               // Add a point to the available pool
        }
        currentPoints = points;                                 // Set current points to the number of points available

        for(int i = enemies.Length - 1; i >= 0; i--)            // For each enemy in the enemies pool
        {
            int cost = enemies[i].GetComponent<Enemy>().points;     // Get cost of the enemy
            if(cost <= points / Mathf.Pow(2, i))                    // If there are enough points available to start spawning the enemy 
            {                                                           // While there are enough points in the current enemy's spawn pool available OR the enemy being spawned is the cheapest one
                while((i == 0 && currentPoints >= cost) || (currentPoints - cost >= points / Mathf.Pow(2, i)))
                {
                    currentPoints -= cost;                                  // Spend the points cost of the enemy
                    SpawnEnemy(enemies[i]);                                 // Spawn the enemy
                }
            }
        }

        countdown = spawnRate;                                  // Reset countdown until next wave
    }

    void SpawnEnemy(GameObject enemy)                       // Spawn enemy
    {
        Vector3 spawnPosition;                                  // Initialise spawn position
        do                                                      // Do
        {
            spawnPosition = player.transform.position;              // Set spawn position to the player's position
            float xPos = Random.Range(-spawnRadius, spawnRadius);   // Get a random value within the spawn radius range
            spawnPosition.x += xPos;                                // Add the value recieved to the x position and add a positive or negative corresponding value to the y position
            spawnPosition.y += (Random.Range(0, 2) * 2 - 1) * Mathf.Sqrt(spawnRadius * spawnRadius - xPos * xPos);      // The resulting position is a random point on a circle with the preset radius 
        }                                                       // Repeat if the position is outside arena bounds
        while (spawnPosition.x > posArenaCorner.x || spawnPosition.y > posArenaCorner.y || spawnPosition.x < negArenaCorner.x || spawnPosition.y < negArenaCorner.y);
                                                                // Instantiate the enemy
        enemy = Instantiate(enemy,spawnPosition, player.transform.rotation);
        enemy.GetComponent<Enemy>().target = player;            // Assign the player as a target for the enemy
    }
}
