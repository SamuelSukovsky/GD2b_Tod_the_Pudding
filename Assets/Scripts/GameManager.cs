using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnRate = 10f;
    public float countdown = 0f;
    public int points = 2;
    private int currentPoints;
    public int pointIncrease = 3;
    private int increasePoints = 0;
    public float spawnRadius;
    public GameObject player;
    public GameObject[] enemies;

    void Awake()
    {
        countdown = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f)
        {
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        increasePoints++;
        if(increasePoints == pointIncrease)
        {
            increasePoints = 0;
            points++;
        }
        currentPoints = points;

        for(int i = enemies.Length - 1; i >= 0; i--)
        {
            int cost = enemies[i].GetComponent<Enemy>().points;
            Debug.Log(cost);
            if(cost <= points / Mathf.Pow(2, i))
            {
                while((i == 0 && currentPoints >= cost) || (currentPoints - cost >= points / Mathf.Pow(2, i)))
                {
                    Debug.Log("Spawning " + enemies[i].name);
                    currentPoints -= cost;
                    SpawnEnemy(enemies[i]);
                }
            }
        }

        countdown = spawnRate;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Vector3 spawnPosition = player.transform.position;
        float xPos = Random.Range(-spawnRadius, spawnRadius);
        spawnPosition.x += xPos;
        spawnPosition.y += (Random.Range(0, 2) * 2 - 1) * Mathf.Sqrt(spawnRadius * spawnRadius - xPos * xPos);

        enemy = Instantiate(enemy,spawnPosition, player.transform.rotation);
        enemy.GetComponent<Enemy>().target = player;
    }
}
