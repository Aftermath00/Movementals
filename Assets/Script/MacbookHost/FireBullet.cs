using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;
    private float trajectoryMaxHeight;
    [SerializeField] Enemy enemy;
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
            UpdateProjectilePosition();

            // Check if the projectile is close enough to the target to be destroyed
            if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
            {
                DealDamage(enemy);  // Deal damage first
                Destroy(gameObject); // Then destroy the projectile
            }
        }
        else
        {
            Debug.LogError("No target assigned. Cannot update projectile position.");
        }
    }

    private void UpdateProjectilePosition()
{
    elapsedTime += Time.deltaTime; // Track time since the projectile was instantiated

    // Calculate the journey length from start to target
    float journeyLength = Vector3.Distance(trajectoryStartPoint, target.position);
    if (journeyLength == 0) return; // Safety check

    // Increase speed based on time passed
    float distanceCovered = elapsedTime * moveSpeed;

    // Ensure the fraction of journey is between 0 and 1
    float fractionOfJourney = Mathf.Clamp01(distanceCovered / journeyLength);

    // Lerp position for X, Y, and Z movement
    Vector3 nextPosition = Vector3.Lerp(trajectoryStartPoint, target.position, fractionOfJourney);

    // Use the trajectoryAnimationCurve to modify the Y axis for a custom height arc
    float heightOffset = trajectoryAnimationCurve.Evaluate(fractionOfJourney) * trajectoryMaxHeight;
    nextPosition.y += heightOffset;

    // Use axisCorrectionAnimationCurve for additional corrections on Y (or X/Z axis if needed)
    float axisCorrectionValue = axisCorrectionAnimationCurve.Evaluate(fractionOfJourney);
    // Optionally, apply the correction to the X or Z axis for more complex movement:
    // nextPosition.x += axisCorrectionValue; // For wobbling or curved paths
    nextPosition.y += axisCorrectionValue; // Adding correction to Y-axis

    // Apply the new position to the projectile
    transform.position = nextPosition;

    Debug.Log("Custom Projectile Position Updated: " + transform.position);
}


    public void InitializeProjectile(Transform target, float moveSpeed, Enemy enemy, float maxHeight, AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve)
{
    this.target = target;
    this.moveSpeed = moveSpeed;
    this.enemy = enemy;
    this.trajectoryMaxHeight = maxHeight; // Ensure this is a float
    this.trajectoryAnimationCurve = trajectoryAnimationCurve; // Animation curve for trajectory
    this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve; // Animation curve for axis correction
}



    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve)
    {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
    }

    public void DealDamage(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.EnemyTakeDamage(attack);
            Debug.Log("Enemy took damage: " + attack);
        }
        else
        {
            Debug.LogError("Enemy object is null, cannot deal damage.");
        }
    }
}
