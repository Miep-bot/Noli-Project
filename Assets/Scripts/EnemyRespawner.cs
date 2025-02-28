using System.Collections;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public float respawnTime = 3f; // Time before enemy reappears
    public GameObject EnemyPrefab;

    public void RespawnEnemy(GameObject enemy, Vector3 spawnPosition)
    {
        StartCoroutine(RespawnCoroutine(enemy, spawnPosition));
    }

    private IEnumerator RespawnCoroutine(GameObject enemy, Vector3 spawnPosition)
    {
        Destroy(enemy);

        yield return new WaitForSeconds(respawnTime); // Wait for respawn time

        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

        // Assign references
        EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            enemyBehavior.respawner = this; // Assign this respawner script to new enemy
        }
    }
}
