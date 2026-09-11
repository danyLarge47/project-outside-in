using System;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

public class BarrierExpand : MonoBehaviour
{
    public float minSize;
    public float maxSize;
    public float currentSize;
    public float growSpeed;

    [Tooltip("How much size is removed each time the barrier is hit.")]
    public float hitReduction = 1f;

    [Tooltip("Damage dealt to the player on contact.")]
    public float damageToPlayer = 2f;

    public Transform barrier;
    public Actions onBarrierBroken;

    private bool isBroken = false;

    private void Start()
    {
        ApplySize();
    }

    private void Update()
    {
        if (isBroken) return;

        // Grow the barrier until it reaches its max size.
        if (currentSize < maxSize)
        {
            currentSize = Mathf.Min(currentSize + growSpeed * Time.deltaTime, maxSize);
            ApplySize();
        }
    }

    // Called by the player's Attack via SendMessage("ApplyDamage", dmgValue).
    public void ApplyDamage(float damage)
    {
        Reduce(Mathf.Abs(damage));
    }

    public void OnHit()
    {
        Reduce(hitReduction);
    }

    private void Reduce(float amount)
    {
        if (isBroken) return;

        // Reduce current size when hit; break once it drops below the min size.
        currentSize -= amount;
        ApplySize();

        if (currentSize <= minSize)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true;
        onBarrierBroken?.Run();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Send damage (and knockback, handled inside PlayerHealth) to the player.
        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damageToPlayer, transform.position);
    }
    private void ApplySize()
    {
        if (barrier != null) barrier.localScale = Vector3.one * currentSize;
    }
}
