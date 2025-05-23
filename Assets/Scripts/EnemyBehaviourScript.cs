using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    private Material enemyMaterial;
    private Color originalColor;
    private Coroutine colorChangeCoroutine;

    // Health-related variables
    [Header("Health Settings")]
    public float maxHealth = 100f;  // Maximum Health
    public float currentHealth;     // Current Health (can be modified in the inspector)
    public GameObject healthBar;    // Reference to the health bar object
    public RectTransform healthBarFill; // Transform for the fill part of the health bar (scale it to show health)
    public EnemyRespawner respawner;
    private Vector3 startPosition;

    public EnemyMovement enemy;

    public float pushBackForce = 10f; // The force with which the enemy is pushed back
    private Rigidbody rb; // The Rigidbody of the enemy

    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy")?.GetComponent<EnemyMovement>();

        // Initialize health
        currentHealth = currentHealth;

        // Get the material component from the enemy
        enemyMaterial = GetComponent<Renderer>().material;
        originalColor = enemyMaterial.color;

        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        if (healthBarFill != null)
        {
            UpdateHealthBar();
        }
    }

    private void OnEnable()
    {
        enemy = GameObject.FindWithTag("Enemy Moving")?.GetComponent<EnemyMovement>();

        // Initialize health
        currentHealth = maxHealth;

        // Get the material component from the enemy
        enemyMaterial = GetComponent<Renderer>().material;
        originalColor = enemyMaterial.color;

        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        if (healthBarFill != null)
        {
            UpdateHealthBar();
        }

        startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            PushBack();

            Debug.Log("player hit");
        }
        else if (collision.gameObject.CompareTag("Fireball"))
        {
            if (colorChangeCoroutine != null) StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = StartCoroutine(FlashColor(collision.gameObject.GetComponent<Renderer>().material.color, 8, 1f, 0.1f));

            TakeDamage(6f);
            PushBack();
        }
        else if (collision.gameObject.CompareTag("IceCube"))
        {
            // **Ensure correct enemy reference**
            EnemyMovement enemyMovement = GetComponent<EnemyMovement>();

            if (enemyMovement != null)
            {
                if (colorChangeCoroutine != null) StopCoroutine(colorChangeCoroutine);
                colorChangeCoroutine = StartCoroutine(FreezeEffect(collision.gameObject.GetComponent<Renderer>().material.color, 4f, enemyMovement));

                TakeDamage(10f);
                PushBack();
            }
        }
    }

    private void PushBack()
    {
        // Get the direction to push the enemy (from the enemy to the player)
        Vector3 pushDirection = transform.position - Camera.main.transform.position; // Assuming player is the camera (XR Origin)
        pushDirection.y = 0;  // Remove the vertical component to only push on the x/z plane

        // Apply a force to push the enemy back
        if (rb != null)
        {
            rb.AddForce(pushDirection.normalized * pushBackForce, ForceMode.Impulse);
        }
    }

    private IEnumerator FlashColor(Color hitColor, int flashes, float interval, float flashTime)
    {
        for (int i = 0; i < flashes; i++)
        {
            enemyMaterial.color = hitColor;
            TakeDamage(2f);
            yield return new WaitForSeconds(flashTime);
            enemyMaterial.color = originalColor;
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator FreezeEffect(Color iceColor, float duration, EnemyMovement enemyMovement)
    {
        enemyMaterial.color = iceColor;

        // **Make sure to slow the correct enemy**
        float originalSpeed = enemyMovement.moveSpeed;
        enemyMovement.moveSpeed *= 0.5f;  // Slow enemy down to half speed

        yield return new WaitForSeconds(duration);

        enemyMaterial.color = originalColor;
        enemyMovement.moveSpeed = originalSpeed; // Restore original speed
    }

    // Function to handle damage and update health
    private void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth != 0)
        {
            if (currentHealth < 0f)
            {
                currentHealth = 0f;
                Die();
            }
        }
        else
        {
            Die();
        }
        

        // Update the health bar
        UpdateHealthBar();
    }

    private void Die()
    {
        // Notify the Respawn Script to handle respawning
        if (respawner != null)
        {
            EnemyIdentifier id = GetComponent<EnemyIdentifier>();
            if (id != null)
            {
                EnemyManager.Instance.UnregisterEnemy(id.enemyID);
            }

            Destroy(gameObject);
        }
    }

    // Function to update the health bar
    // Function to update the health bar
    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Calculate the fill based on current health and max health
            float healthPercentage = currentHealth;
            Debug.Log("Health Percentage: " + healthPercentage);  // Debugging line

            // Assuming the health bar is using a RectTransform for scaling
            healthBarFill.sizeDelta = new Vector2(healthPercentage * 2, healthBarFill.sizeDelta.y);  // Scale the health bar
        }
    }
}
