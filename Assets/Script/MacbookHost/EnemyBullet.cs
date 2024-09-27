using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;
    private float trajectoryMaxHeight;
    [SerializeField] TreeScript tree;
    private float distanceToTargetToDestroyProjectile = 1f;
    [SerializeField] private float attack;
    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve;
    private Vector3 trajectoryStartPoint;
    private float elapsedTime = 0f;

    private void Start()
    {
        trajectoryStartPoint = transform.position;

        if (target == null)
        {
            Debug.LogError("Target not set for the projectile!");
        }
    }

    private void Update()
    {
        if (target != null) // Check if target is assigned
        {
            UpdateEnemyProjectilePosition();

            // Check if the projectile is close enough to the target to be destroyed
            if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
            {
                Destroy(gameObject); // Then destroy the projectile

                DealDamage(tree);  // Deal damage first
            }
        }
        else
        {
            Debug.LogError("No target assigned. Cannot update projectile position.");
        }
    }

    private void UpdateEnemyProjectilePosition()
    {
        elapsedTime += Time.deltaTime; // Track time since the projectile was instantiated

        // Calculate the journey length from start to target
        float journeyLength = Vector3.Distance(trajectoryStartPoint, target.position);
        if (journeyLength == 0) return; // Safety check

        // Increase speed based on time passed
        float distanceCovered = elapsedTime * moveSpeed;

        // Make sure to handle edge cases where fraction might exceed 1
        float fractionOfJourney = Mathf.Clamp01(distanceCovered / journeyLength); // Ensure it stays between 0 and 1

        // Lerp position (linear interpolation) for X and Z movement
        Vector3 nextPosition = Vector3.Lerp(trajectoryStartPoint, target.position, fractionOfJourney);

        // Modify the Y position using the trajectoryAnimationCurve (controls the arc)
        float trajectoryNormalizedTime = fractionOfJourney; // This is from 0 to 1
        float heightOffset = trajectoryAnimationCurve.Evaluate(trajectoryNormalizedTime);

        // Add height offset to Y value
        nextPosition.y += heightOffset;

        // Optional: Add additional axis correction if needed (for more complex movement)
        float axisCorrectionValue = axisCorrectionAnimationCurve.Evaluate(trajectoryNormalizedTime);
        nextPosition.y += axisCorrectionValue; // You can also apply this to the X or Z axis if needed

        // Apply the new position to the projectile
        transform.position = nextPosition;

        Debug.Log("Projectile Position Updated: " + transform.position);
    }

    public void InitializeEnemyProjectile(Transform target, float moveSpeed, TreeScript tree, float projectileMaxHeight, AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.tree = tree;
        this.trajectoryAnimationCurve = trajectoryAnimationCurve; // Set the trajectory curve
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;

        Debug.Log("Projectile Initialized: " + moveSpeed);
    }

    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve)
    {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
    }

    public void DealDamage(TreeScript tree)
    {
        if (tree != null)
        {
            tree.PlayerTakeDamage(attack);
            Debug.Log("Player took damage: " + attack);
        }
        else
        {
            Debug.LogError("Player object is null, cannot deal damage.");
        }
    }
}
