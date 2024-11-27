using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBoss : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public HealthbarBehaviour healthbar;
    public bool isDisabled = false;
    private Vector3 originalScale;
    private Animator animator; // Referensi ke Animator

    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;

        if (healthbar != null)
        {
            healthbar.SetHealth(health, maxHealth);
        }

        originalScale = transform.localScale;
    }


    // Update is called once per frame
    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        if (healthbar != null)
        {
            healthbar.SetHealth(health, maxHealth);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 0.2f);
    }

    public IEnumerator DisableEnemy(float duration)
    {
        isDisabled = true;
        animator.SetTrigger("isDisabled");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(duration);

        rb.isKinematic = false;
        isDisabled = false;
        animator.SetTrigger("isReactivated");
    }
}

