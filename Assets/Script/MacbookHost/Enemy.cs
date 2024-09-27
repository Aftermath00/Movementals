using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] float health, maxHealth = 100f;

    [SerializeField] EnemyHealthBar healthBar;

    

    // Start is called before the first frame update

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

    }
    private void Awake()
    {
         healthBar = GetComponentInChildren<EnemyHealthBar>(); 
    }

    public void EnemyTakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    
}
