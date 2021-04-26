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
    private float[] alienSpawnWeighting;
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
    public float variableSpawnActivatePercentage;
    public float variableSpawnMultiplier;
    private int maxAllowableAliens;
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
            timeUntilSpawn = getSpawnTime();
        }
    }

    private float getSpawnTime() {
        float multiplier = spawnMultiplier;
        float rateIncreasePoint = (float)maxAllowableAliens * variableSpawnActivatePercentage;
        if ((float)currentActiveAliensSO.Value < rateIncreasePoint && variableSpawnMultiplier > 0) {
            multiplier *= (1/variableSpawnMultiplier);
        }
        float minTime = minSpawnCooldown*multiplier;
        float maxTime = maxSpawnCooldown*multiplier;
        float spawnTime = Random.Range(minTime, maxTime);
        return spawnTime;
    }

    private GameObject getRandomAlienType() {
        float weightTotal = 0;
        for (int i = 0; i < activeAlienPrefabs.Count; i++) {
            weightTotal += alienSpawnWeighting[i];
        }
        float randomNum = Random.value;
        float sum = 0f;
        int selectedIndex = 0;
        for (int j = 0; j < activeAlienPrefabs.Count; j++) {
            sum += (alienSpawnWeighting[j]/weightTotal);
            if (sum >= randomNum) {
                selectedIndex = j;
                break;
            }
        }
        GameObject chosenAlienPrefab = activeAlienPrefabs[selectedIndex];
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

        // Get a random rotation
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        // Spawn the alien
        GameObject spawnedAlien = Instantiate(chosenAlienPrefab, spawnPos, rotation) as GameObject;
        Rigidbody2D spawnRb;
        if (spawnedAlien.GetComponent<MultiMorp>() != null) {
            spawnRb = spawnedAlien.transform.GetChild(0).GetComponent<Rigidbody2D>();
            spawnDir *= 2;
            currentActiveAliensSO.Value += spawnedAlien.GetComponent<MultiMorp>().morpsConnected.Length;
        }
        else {
            spawnRb = spawnedAlien.GetComponent<Rigidbody2D>();
            currentActiveAliensSO.Value++;
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
        if (currentActiveAliensSO.Value == maxAllowableAliens) {
            timeUntilSpawn = getSpawnTime();
        }
        currentActiveAliensSO.Value--;
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
