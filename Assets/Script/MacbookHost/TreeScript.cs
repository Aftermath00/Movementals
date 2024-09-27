using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;

public class TreeScript : MonoBehaviour
{

     [SerializeField] float health, maxHealth = 100f;

    [SerializeField] EnemyHealthBar healthBar;

 private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

    }
    private void Awake()
    {
         healthBar = GetComponentInChildren<EnemyHealthBar>(); 
    }

    public void PlayerTakeDamage(float damageAmount)
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
