using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShootVariation : ShootVariation
{
    [SerializeField] private float AdditionalDamage;
    [SerializeField] private float AdditionalShootDelay;
    [SerializeField] private float TimeBeforeShoot;

    private void Update()
    {
        if (Weapon.Instance.WeaponAmmo != 0) Weapon.Instance.WeaponAmmo = 1;
    }

    public override float Shoot(Transform position, Transform direction)
    {
        Shot shot = Instantiate(shotPrefab, position.position, Quaternion.identity).GetComponent<Shot>();
        shot.Work(direction, position, collideLayer);
        shot.Damage += AdditionalDamage;
        shot.TimeBeforeShot = TimeBeforeShoot;

        return AdditionalShootDelay;
    }
}
