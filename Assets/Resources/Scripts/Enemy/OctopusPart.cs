using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OctopusPart : Enemy
{
    public override void GetDamage(int damage)
    {
        hp -= damage;
    }

    public override void Follow(Transform player)
    {
        return;
    }

    public override void OnDestroy()
    {
        return;
    }
}
