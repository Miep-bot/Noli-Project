using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenTrigger : MonoBehaviour
{
    public EnemyRespawner respawner;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball") || collision.gameObject.CompareTag("IceCube"))
        {
            if (respawner != null)
            {
                respawner.SpawnQueen();
            }

            Destroy(gameObject); // Triggers enemy spawn and destroys this object
        }
    }
}
