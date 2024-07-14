using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShootVariation : ShootVariation
{
    [SerializeField] private int additionalDamage;
    [SerializeField] private float additionalReloadTime;
    [SerializeField] private float timeBeforeShoot;

    private void Update()
    {
        if (Weapon.Instance.CurrectWeaponAmmo != 0) Weapon.Instance.CurrectWeaponAmmo = 1;
        Weapon.Instance.AdditionalReloadTime = additionalReloadTime;
    }

    public override float Shoot(Transform position, Transform direction)
    {
        Shot shot = Instantiate(shotPrefab, position.position, Quaternion.identity).GetComponent<Shot>();
        shot.Work(direction, position, collideLayer);
        shot.Damage += additionalDamage;
        shot.TimeBeforeShot = timeBeforeShoot;

        return 0;
    }
}
