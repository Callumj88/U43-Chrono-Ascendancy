using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The prefab of the enemy to spawn
    public float spawnInterval = 2f;  // Time between spawns
    public float spawnRadius = 5f;    // Radius around the spawner where enemies will be spawned
    public int maxEnemies = 10;       // Maximum number of enemies that can be spawned

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddSpawner();
        }
        StartCoroutine(SpawnEnemies());
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoveSpawner();
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Remove destroyed enemies from the list
            spawnedEnemies.RemoveAll(enemy => enemy == null);

            if (spawnedEnemies.Count < maxEnemies)
            {
                Vector3 spawnPosition = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                InitializeEnemy(enemy);
                spawnedEnemies.Add(enemy);
            }
        }
    }

    void InitializeEnemy(GameObject enemy)
    {
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enemyData = enemyPrefab.GetComponent<EnemyMovement>().enemyData;  // Assuming enemyPrefab has EnemyMovement component
        }
    }
}
