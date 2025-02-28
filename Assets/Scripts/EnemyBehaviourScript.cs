using System.Collections;
using UnityEngine;

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

    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy")?.GetComponent<EnemyMovement>();

        // Initialize health
        currentHealth = maxHealth;

        // Get the material component from the enemy
        enemyMaterial = GetComponent<Renderer>().material;
        originalColor = enemyMaterial.color;

        if (healthBarFill != null)
        {
            UpdateHealthBar();
        }
    }

    private void OnEnable()
    {
        enemy = GameObject.FindWithTag("Enemy")?.GetComponent<EnemyMovement>();

        // Initialize health
        currentHealth = maxHealth;

        // Get the material component from the enemy
        enemyMaterial = GetComponent<Renderer>().material;
        originalColor = enemyMaterial.color;

        if (healthBarFill != null)
        {
            UpdateHealthBar();
        }

        startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Handle the fireball hit
            if (colorChangeCoroutine != null) StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = StartCoroutine(FlashColor(collision.gameObject.GetComponent<Renderer>().material.color, 8, 1f, 0.1f));

            // Deduct health on fireball hit
            TakeDamage(6f);  // For example, fireball does 10 damage
        }
        else if (collision.gameObject.CompareTag("IceCube"))
        {
            // Handle the ice cube hit
            if (colorChangeCoroutine != null) StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = StartCoroutine(FreezeEffect(collision.gameObject.GetComponent<Renderer>().material.color, 4f));

            // Deduct health on ice cube hit
            TakeDamage(10f);   // For example, ice cube does 5 damage
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

    private IEnumerator FreezeEffect(Color iceColor, float duration)
    {
        enemyMaterial.color = iceColor;
        enemy.moveSpeed = 0.5f;
        yield return new WaitForSeconds(duration);
        enemyMaterial.color = originalColor;
        enemy.moveSpeed = 2f;
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
            respawner.RespawnEnemy(gameObject, startPosition);
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
