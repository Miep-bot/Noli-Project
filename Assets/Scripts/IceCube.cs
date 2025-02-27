using UnityEngine;

public class IceCube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the ice cube hits something other than the controller
        if (!collision.gameObject.CompareTag("Controller"))
        {
            Debug.Log("Ice cube hit: " + collision.gameObject.name);
            Destroy(gameObject); // Destroy ice cube on impact
        }
    }
}
