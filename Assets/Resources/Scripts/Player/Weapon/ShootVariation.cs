using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootVariation : MonoBehaviour
{

    [SerializeField] protected GameObject shotPrefab;
    [SerializeField] protected LayerMask collideLayer;

    public virtual float Shoot(Transform position, Transform direction)
    {
        return 0;
    }
}
