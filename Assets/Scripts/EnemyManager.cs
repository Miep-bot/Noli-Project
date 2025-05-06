using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public MoveObjectUp objectToMoveUp;  // Assign this via Inspector
    public MoveObjectUp objectToMoveUp2;

    public GameObject objectToReveal;

    private int nextEnemyID = 1;
    private Dictionary<int, GameObject> activeEnemies = new Dictionary<int, GameObject>();
    private HashSet<int> enemiesThatHaveDied = new HashSet<int>();

    // Track first two enemies
    private List<int> firstTwoEnemyIDs = new List<int>();
    private bool triggeredFirstTwoDeathEvent = false;

    private List<int> thirdToSixthEnemyIDs = new List<int>();
    private bool triggeredThirdToSixthDeathEvent = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int RegisterEnemy(GameObject enemy)
    {
        int id = nextEnemyID++;
        activeEnemies[id] = enemy;

        EnemyIdentifier identifier = enemy.GetComponent<EnemyIdentifier>();
        if (identifier != null)
        {
            identifier.enemyID = id;
        }

        if (firstTwoEnemyIDs.Count < 2)
        {
            firstTwoEnemyIDs.Add(id);
        }
        else if (thirdToSixthEnemyIDs.Count < 4) // 3rd to 6th enemies
        {
            thirdToSixthEnemyIDs.Add(id);
        }

        Debug.Log($"Enemy {id} spawned.");
        return id;
    }

    public void UnregisterEnemy(int id)
    {
        if (activeEnemies.ContainsKey(id))
        {
            activeEnemies.Remove(id);
        }

        enemiesThatHaveDied.Add(id);
        Debug.Log($"Enemy {id} died.");

        // Check if the first two enemies have both died
        if (!triggeredFirstTwoDeathEvent && firstTwoEnemyIDs.TrueForAll(id => enemiesThatHaveDied.Contains(id)))
        {
            triggeredFirstTwoDeathEvent = true;
            OnFirstTwoEnemiesDied();
        }

        // Check if enemies 3–6 have all died
        if (!triggeredThirdToSixthDeathEvent && thirdToSixthEnemyIDs.Count == 4 &&
            thirdToSixthEnemyIDs.TrueForAll(id => enemiesThatHaveDied.Contains(id)))
        {
            triggeredThirdToSixthDeathEvent = true;
            OnThirdToSixthEnemiesDied();
        }
    }

    private void OnFirstTwoEnemiesDied()
    {
        Debug.Log("The first two enemies have died!");
        if (objectToMoveUp != null)
        {
            objectToMoveUp.StartRising();

            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
            }
        }
    }

    private void OnThirdToSixthEnemiesDied()
    {
        Debug.Log("Enemies 3 to 6 have died!");
        if (objectToMoveUp != null)
        {
            objectToMoveUp2.StartRising();
        }
    }
}
