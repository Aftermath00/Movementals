using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;
    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private float projectileMaxHeight;
    [SerializeField] TreeScript tree;
    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionANimationCurve;
    [SerializeField] private float fireInterval = 1f;
    private float fireCooldown;
    private void Start()
    {
        fireCooldown = fireInterval; // Initialize the cooldown to the fire interval
    }

    private void Update()
    {
        // Decrease the cooldown timer
        fireCooldown -= Time.deltaTime;

        // Check if the cooldown timer has reached zero or below
        if (fireCooldown <= 0f)
        {
            FireProjectile(); // Fire the projectile
            fireCooldown = fireInterval; // Reset the cooldown timer to the fire interval (5 seconds)
        }
    }

     private void FireProjectile()
    {
        // Instantiate the projectile
         EnemyBullet projectile =  Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyBullet>();
            projectile.InitializeEnemyProjectile(target, projectileMoveSpeed, tree, projectileMaxHeight, trajectoryAnimationCurve, axisCorrectionANimationCurve);
            projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionANimationCurve);
    }
}
