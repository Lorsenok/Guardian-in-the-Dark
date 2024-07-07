using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShootVariation : ShootVariation
{
    public override float Shoot(Transform position, Transform direction)
    {
        GameObject shot = Instantiate(shotPrefab, position.position, Quaternion.identity);
        shot.GetComponent<Shot>().Work(direction, position, collideLayer);
        return 0;
    }
}
