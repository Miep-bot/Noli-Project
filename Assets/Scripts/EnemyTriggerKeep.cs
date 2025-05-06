using UnityEngine;

public class SpawnTrigger2 : MonoBehaviour
{
    public EnemyRespawner respawner;

    public GameObject objectToHide;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball") || collision.gameObject.CompareTag("IceCube"))
        {
            if (objectToHide != null)
            {
                objectToHide.SetActive(false);  // Make it visible
            }

            if (respawner != null)
            {
                respawner.SpawnEnemiesArea2();
            }

            Destroy(gameObject); // Triggers enemy spawn and destroys this object
        }
    }
}
