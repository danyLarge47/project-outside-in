using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [System.Serializable]
    public class HealthEvent : UnityEvent<float> { }

    [Header("Health")]
    public float maxHealth = 10f;
    public float currentHealth;

    [Header("Invincibility")]
    [Tooltip("Seconds the player cannot take damage after being hit.")]
    public float invincibleTime = 1f;
    public bool invincible = false;

    [Header("Knockback")]
    [Tooltip("Force applied away from the damage source when hit.")]
    public float knockbackForce = 400f;

    [Header("Events")]
    [Tooltip("Fires with the normalized health (0..1) whenever it changes.")]
    public HealthEvent onHealthChanged;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    private Rigidbody2D rb;
    private bool isDead = false;

    public bool IsDead => isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        onHealthChanged?.Invoke(GetHealthNormalized());
    }

    public void TakeDamage(float damage, Vector3 sourcePosition)
    {
        if (isDead || invincible || damage <= 0f) return;

        currentHealth -= damage;
        onDamaged?.Invoke();

        ApplyKnockback(sourcePosition);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            onHealthChanged?.Invoke(GetHealthNormalized());
            Die();
        }
        else
        {
            onHealthChanged?.Invoke(GetHealthNormalized());
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke(GetHealthNormalized());
    }

    public float GetHealthNormalized()
    {
        return maxHealth <= 0f ? 0f : currentHealth / maxHealth;
    }

    private void ApplyKnockback(Vector3 sourcePosition)
    {
        if (rb == null) return;

        Vector2 direction = ((Vector2)(transform.position - sourcePosition)).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce);
    }

    private void Die()
    {
        isDead = true;
        onDeath?.Invoke();
    }

    private IEnumerator InvincibilityRoutine()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }
}
