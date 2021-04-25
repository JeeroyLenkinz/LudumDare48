﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AlienSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform leftBound;
    [SerializeField]
    private Transform rightBound;
    [SerializeField]
    private GameObject[] alienPrefabs;
    [SerializeField]
    private IntReference currentActiveAliensSO;
    private float timeUntilSpawn;
    
    public float minAngle;
    public float maxAngle;
    public float minForce;
    public float maxForce;
    public float minSpawnCooldown;
    public float maxSpawnCooldown;
    public int maxAllowableAliens;

    
   public void e_alienDropped() {
       currentActiveAliensSO.Value--;
       timeUntilSpawn = Random.Range(minSpawnCooldown, maxSpawnCooldown);
   }
   
    // Start is called before the first frame update
    void Start()
    {
        currentActiveAliensSO.Value = 0;
        timeUntilSpawn = minSpawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0 && currentActiveAliensSO.Value < maxAllowableAliens) {
            GameObject chosenAlienType = chooseAlienType();
            spawnAlien(chosenAlienType);
            timeUntilSpawn = Random.Range(minSpawnCooldown, maxSpawnCooldown);
            currentActiveAliensSO.Value++;
        }
    }

    private GameObject chooseAlienType() {
        GameObject chosenAlienPrefab = alienPrefabs[Random.Range(0, alienPrefabs.Length)];
        return chosenAlienPrefab;
    }

    private void spawnAlien(GameObject chosenAlienPrefab) {
        // Get a random X position
        float xMin = leftBound.position.x;
        float xMax = rightBound.position.x;
        float yPos = leftBound.position.y;
        Vector3 spawnPos = new Vector3(Random.Range(xMin, xMax), yPos, 0);

        // Get a random velocity
        float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        spawnDir = spawnDir * Random.Range(minForce, maxForce);

        GameObject spawnedAlien = Instantiate(chosenAlienPrefab, spawnPos, Quaternion.identity) as GameObject;
        Rigidbody2D spawnRb = spawnedAlien.GetComponent<Rigidbody2D>();
        spawnRb.velocity = spawnDir;
    }
}
