using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedEnemy : Enemy
{
    [Header("Winged")]
    [SerializeField] private float attackTime;

    [SerializeField] private float attackForce;
    [SerializeField] private float attackForceDecreaseSpeed;

    [SerializeField] private float attackDelay;
    [SerializeField] private float additionalAttackDelay;

    [SerializeField] private float attackRange;
    [SerializeField] private float attackReturnSpeed;
    [SerializeField] private int attackQuantity;

    [SerializeField] private Transform firstAttack;
    [SerializeField] private Transform secondAttack;

    private float firstAttackStartAngle;
    private float secondAttackStartAngle;

    private bool hasFirstAttackStarted = false;
    private bool hasSecondAttackStarted = false;
    private bool attackSwitch = false;

    private float curAttackDelay;
    private float curAdditionalAttackDelay;

    private int curAttackQuantity;

    private float curAttackTime = 0;
    private float curAttackForce = 0;

    public override void Follow(Transform player)
    {
        base.Follow(player);
        follow.enabled = curAttackQuantity <= 0 && curAttackTime <= 0;
    }

    public override void Start()
    {
        curAttackDelay = attackDelay;

        firstAttackStartAngle = firstAttack.eulerAngles.z;
        secondAttackStartAngle = secondAttack.eulerAngles.z;

        base.Start();
    }

    private bool Attack(Transform obj, float startAngle, int m)
    {
        if (curAttackTime >= attackTime)
        {
            curAttackTime = 0;
            return false;
        }

        obj.transform.localRotation = Quaternion.Euler(0, 0, (startAngle * m + attackRange / attackTime * curAttackTime) * m);
        curAttackTime += Time.deltaTime;

        return true;
    }

    public override void Update()
    {
        base.Update();

        if (hp <= 0) return;

        if (curAttackQuantity <= 0 && curAttackTime <= 0) curAttackDelay -= Time.deltaTime;
        if (curAttackQuantity > 0 && curAttackTime <= 0) SmoothRotateTowardsPosition(speed, player.position);

        if (curAttackDelay <= 0)
        {
            curAttackQuantity = attackQuantity;
            curAttackDelay = attackDelay;
        }

        if (curAttackQuantity > 0)
        {
            curAdditionalAttackDelay -= Time.deltaTime;
        }

        if (curAdditionalAttackDelay <= 0 && curAttackQuantity > 0)
        {
            curAdditionalAttackDelay = additionalAttackDelay;
            curAttackQuantity -= 1;
            attackSwitch = !attackSwitch;

            curAttackForce = attackForce;

            if (attackSwitch) hasFirstAttackStarted = true;
            else hasSecondAttackStarted = true;
        }

        if (hasFirstAttackStarted) hasFirstAttackStarted = Attack(firstAttack, firstAttackStartAngle, -1);
        else firstAttack.transform.localRotation = Quaternion.Lerp(firstAttack.transform.localRotation, Quaternion.Euler(0, 0, firstAttackStartAngle), Time.deltaTime * attackReturnSpeed);

        if (hasSecondAttackStarted) hasSecondAttackStarted = Attack(secondAttack, secondAttackStartAngle, 1);
        else secondAttack.transform.localRotation = Quaternion.Lerp(secondAttack.transform.localRotation, Quaternion.Euler(0, 0, secondAttackStartAngle), Time.deltaTime * attackReturnSpeed);

        curAttackForce = Mathf.Lerp(curAttackForce, 0, Time.deltaTime * attackForceDecreaseSpeed);

        transform.position += (player.position - transform.position).normalized * curAttackForce * Time.deltaTime;
    }
}
