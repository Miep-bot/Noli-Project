using UnityEngine;
using UnityEngine.SceneManagement;  // For reloading the scene

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum player health
    public float currentHealth;   // Current health of the player
    public GameObject healthBar;    // Reference to the health bar object
    public RectTransform healthBarFill;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        UpdateHealthBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10f);  // Lose 20 health per enemy touch (adjust as needed)
            UpdateHealthBar();
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart scene
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Calculate the fill based on current health and max health
            float healthPercentage = currentHealth;
            Debug.Log("Health Percentage: " + healthPercentage);  // Debugging line

            // Assuming the health bar is using a RectTransform for scaling
            healthBarFill.sizeDelta = new Vector2(healthPercentage / 50, healthBarFill.sizeDelta.y);  // Scale the health bar
        }
    }
}
