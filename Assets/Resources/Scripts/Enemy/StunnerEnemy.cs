using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StunnerEnemy : Enemy
{
    [Header("Stunner")]
    [SerializeField] private float speedSet;
    [SerializeField] private float startShakePower;

    private bool hasStunnedPlayer = false;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2 * damage * damage;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (!hasStunnedPlayer)
        {
            hasStunnedPlayer = true;
            Shake(startShakePower);
        }

        Controller.Instance.CurrectSpeed = speedSet;
    }
}
