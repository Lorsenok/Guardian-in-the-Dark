using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StunnerEnemy : Enemy
{
    [Header("Stunner")]
    [SerializeField] private float speedSet;
    [SerializeField] private float startShakePower;
    [SerializeField] private AudioSource appearSound;

    private bool hasStunnedPlayer = false;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2 * damage;
    }

    public override void Start()
    {
        appearSound.volume = Config.Sound;
        appearSound.Play();

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
    }

    public void LateUpdate()
    {
        Controller.Instance.CurrectSpeed = speedSet;
    }
}
