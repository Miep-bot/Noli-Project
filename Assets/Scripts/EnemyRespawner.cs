using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject QueenPrefab;
    public List<Transform> spawnPoints; // Drag empty GameObjects here in the inspector
    public List<Transform> spawnPointsArea2;
    public List<Transform> spawnPointsArea3;

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

            EnemyManager.Instance.RegisterEnemy(newEnemy);
        }
    }

    public void SpawnEnemiesArea2()
    {
        foreach (Transform spawnPoint in spawnPointsArea2)
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

            EnemyManager.Instance.RegisterEnemy(newEnemy);
        }
    }

    public void SpawnQueen()
    {
        foreach (Transform spawnPoint in spawnPointsArea3)
        {
            GameObject newEnemy = Instantiate(QueenPrefab, spawnPoint.position, Quaternion.identity);

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

            EnemyManager.Instance.RegisterEnemy(newEnemy);
        }
    }
}
