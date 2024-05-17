using System.Collections;
using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the player's stats
    public float invincibilityDuration = 0.5f;
    public TextMeshProUGUI healthText;  // Reference to the UI Text displaying health

    private float currentHealth;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = playerStats.MaxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            StartCoroutine(InvincibilityCoroutine());
            if (currentHealth <= 0)
            {
                Die();
            }
            UpdateHealthUI();
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + "HP";
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }
}
