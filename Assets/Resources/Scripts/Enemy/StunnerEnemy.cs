using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnerEnemy : Enemy
{
    [Header("Stunner")]
    [SerializeField] private float speedSet;
    [SerializeField] private float startShakePower;

    private bool hasStunnedPlayer = false;
    private Controller controller;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2 * damage;
    }

    public override void Start()
    {
        base.Start();
        controller = player.GetComponentInChildren<Controller>();
    }

    public override void Update()
    {
        base.Update();

        if (!hasStunnedPlayer)
        {
            hasStunnedPlayer = true;
            Shake(startShakePower);
        }

        controller.CurrectSpeed = speedSet;

    }
}
