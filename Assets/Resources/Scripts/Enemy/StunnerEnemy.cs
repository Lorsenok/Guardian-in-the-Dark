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

    [SerializeField] private Light[] lights;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2 * damage * damage;
    }

    public override void Start()
    {
        base.Start();
        controller = player.GetComponentInChildren<Controller>();
    }

    public override void Die()
    {
        base.Die();
        foreach (LightSet3D light in GetComponentsInChildren<LightSet3D>())
        {
            light.enabled = false;
        }
        foreach (Light l in lights)
        {
            l.intensity = Mathf.Lerp(l.intensity, 0, Time.deltaTime);
        }
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
