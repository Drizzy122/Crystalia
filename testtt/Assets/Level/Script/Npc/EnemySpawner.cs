using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnInterval = 1f; // The time interval between spawns
    public float spawnDistance = 10f; // The distance from the spawner at which to spawn the enemy

    private float timeSinceLastSpawn = 0f; // The time since the last enemy was spawned

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // If it's time to spawn a new enemy
        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;

            // Calculate a random position around the spawner to spawn the enemy
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnDistance;

            // Instantiate the enemy prefab at the spawn position
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Set the enemy's parent to this object for organization purposes
            newEnemy.transform.parent = transform;
        }
    }
}
