using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public NavMeshSurface navMeshSurface;
    Collider terrainCollider;

    public float spawnDelay = 2f;
    public int maxEnemies = 10;

    private int currentEnemies = 0;
    private float timer = 0f;

    private NavMeshTriangulation navMesh;
    private NavMeshHit hit;
    private Vector3 position = Vector3.zero;
    private bool found = false;

    void Start()
    {
        //navMeshSurface.BuildNavMesh(); // build the NavMesh surface
        // Get the bounds of the terrain collider
        terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
        navMesh = NavMesh.CalculateTriangulation();
    }
        

    void Update()
    {
        // only spawn enemies if we haven't reached our maximum yet
        if (currentEnemies < maxEnemies)
        {
            // increment the timer
            timer += Time.deltaTime;

            // check if enough time has passed to spawn another enemy
            if (timer >= spawnDelay)
            {
                // reset the timer
                timer = 0f;

                // spawn the enemy at a random position on the NavMesh surface
                Vector3 spawnPosition = GetRandomNavMeshPosition();
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                // increment the enemy count
                currentEnemies++;
            }
        }
    }

    // get a random position on the NavMesh surface
    private Vector3 GetRandomNavMeshPosition()
    {
        found = false;

        // Get the bounds of the active terrain
        Bounds bounds = terrainCollider.bounds;

        // keep trying to find a random position on the NavMesh until we succeed
        while (!found)
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(20f, 35f),
                Random.Range(bounds.min.z, bounds.max.z));

            found = NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
        }

        return hit.position;
    }

    // called when an enemy is destroyed
    public void OnEnemyDestroyed()
    {
        currentEnemies--;
    }

}
