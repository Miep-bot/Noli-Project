using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public EnemyRespawner respawner;

    public GameObject objectToReveal;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);  // Make it visible
            }

            if (respawner != null)
            {
                respawner.SpawnEnemies();
            }

            Destroy(gameObject); // Triggers enemy spawn and destroys this object
        }
    }
}
