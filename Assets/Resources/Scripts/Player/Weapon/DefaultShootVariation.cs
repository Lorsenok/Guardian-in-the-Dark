using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShootVariation : ShootVariation
{
    [SerializeField] private float TimeBeforeShoot;

    public override float Shoot(Transform position, Transform direction)
    {
        Shot shot = Instantiate(shotPrefab, position.position, Quaternion.identity).GetComponent<Shot>();
        shot.Work(direction, position, collideLayer);
        shot.TimeBeforeShot = TimeBeforeShoot;
        return 0;
    }
}
