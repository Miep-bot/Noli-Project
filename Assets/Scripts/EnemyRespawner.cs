using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<Transform> spawnPoints; // Drag empty GameObjects here in the inspector

    public void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint.position, Quaternion.identity);

            EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.respawner = this;
            }

            EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.CanMove = true; // You can customize this if needed
            }
        }
    }
}
