using System.Collections;
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
    private GameObject[] allAlienPrefabs;
    [SerializeField]
    private IntReference currentActiveAliensSO;
    private float timeUntilSpawn;
    private float spawnMultiplier;
    private bool dontSpawn;
    private List<GameObject> activeAlienPrefabs = new List<GameObject>();
    
    public float minAngle;
    public float maxAngle;
    public float minForce;
    public float maxForce;
    public float minSpawnCooldown;
    public float maxSpawnCooldown;
    public int maxAllowableAliens;
    public float groupSpawnDelay;
   
    // Start is called before the first frame update
    void Start()
    {
        currentActiveAliensSO.Value = 0;
        spawnMultiplier = 1;
        timeUntilSpawn = minSpawnCooldown;
        dontSpawn = true;
        activeAlienPrefabs.Add(allAlienPrefabs[0]);
        activeAlienPrefabs.Add(allAlienPrefabs[1]);
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0 && currentActiveAliensSO.Value < maxAllowableAliens && !dontSpawn) {
            GameObject chosenAlienType = getRandomAlienType();
            spawnAlien(chosenAlienType);
            timeUntilSpawn = Random.Range(minSpawnCooldown*spawnMultiplier, maxSpawnCooldown*spawnMultiplier);
            currentActiveAliensSO.Value++;
        }
    }

    private GameObject getRandomAlienType() {
        GameObject chosenAlienPrefab = activeAlienPrefabs[Random.Range(0, activeAlienPrefabs.Count)];
        return chosenAlienPrefab;
    }

    private GameObject getSpecificAlienType(int index) {
        GameObject chosenAlienPrefab = activeAlienPrefabs[index];
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
        Rigidbody2D spawnRb;
        if (spawnedAlien.GetComponent<MultiMorp>() != null) {
            spawnRb = spawnedAlien.transform.GetChild(0).GetComponent<Rigidbody2D>();
        }
        else {
            spawnRb = spawnedAlien.GetComponent<Rigidbody2D>();
        }
        spawnRb.velocity = spawnDir;
    }

    private IEnumerator spawnGroup(int typeIndex, int amount) {
        GameObject alienType = getSpecificAlienType(typeIndex);
        for (int i = 0; i < amount; i++) {
            spawnAlien(alienType);
            yield return new WaitForSeconds(groupSpawnDelay);
        }
    }

    public void e_alienDropped() {
       currentActiveAliensSO.Value--;
       timeUntilSpawn = Random.Range(minSpawnCooldown, maxSpawnCooldown);
    }

    public void e_SetMaxAllowableAliens(int newMax) {
        maxAllowableAliens = newMax;
    }

    public void e_SetSpawnMultiplier(float newMultiplier) {
        if (newMultiplier == 0f) {
            dontSpawn = true;
        }
        else {
            spawnMultiplier = (1/newMultiplier);
            dontSpawn = false;
        }
    }

    public void e_addAlienType(int prefabIndex) {
        activeAlienPrefabs.Add(allAlienPrefabs[prefabIndex]);
    }

    public void e_fixedSpawn(Vector2 spawnInfo) {
        int typeIndex = (int) spawnInfo.x;
        int amount = (int) spawnInfo.y;
        StartCoroutine(spawnGroup(typeIndex, amount));
    }
}
