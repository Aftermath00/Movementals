using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;

public class Player2Script : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;
    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private float projectileMaxHeight;
    [SerializeField] private float maxHeight = 5f; 
     [SerializeField] Enemy enemy;
     [SerializeField] private AnimationCurve trajectoryAnimationCurve;
     [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)) {
            FireBullet projectile =  Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<FireBullet>();
             projectile.InitializeProjectile(target, projectileMoveSpeed, enemy, maxHeight, trajectoryAnimationCurve, axisCorrectionAnimationCurve);
            projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve);
        }
    }
}
