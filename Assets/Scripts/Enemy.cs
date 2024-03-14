using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Animator _animator;
    private bool _isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!_isDead)
        {
            _isDead = true;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}
