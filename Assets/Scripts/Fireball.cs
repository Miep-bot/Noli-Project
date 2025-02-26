using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the fireball hits something other than the controller
        if (!collision.gameObject.CompareTag("Controller"))
        {
            Debug.Log("Fireball hit: " + collision.gameObject.name);
            Destroy(gameObject); // Destroy fireball on impact
        }
    }
}
