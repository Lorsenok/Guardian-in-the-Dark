using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public static ShootVariation shootVariation;

    public bool IsHoldingWeapon = false;

    private Lidar lidar;

    [SerializeField] private float delaySet;
    public float Delay { get; set; }

    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform shootDirection;

    [SerializeField] private float shakePower;

    private CinemachineImpulseSource weaponShake;

    public float SpeedMultiplier { get; set; }
    public float WeaponDamage { get; set; }
    public float WeaponShootDelay { get; set; }
    public float WeaponAccuracy { get; set; }
    public int WeaponAmmo { get; set; }
    public float CurrectWeaponAmmo { get; set; }
    public float WeaponReloadTime { get; set; }
    public float AdditionalReloadTime { get; set; } = 0;
    public float CurrectWeaponReloadTime { get; set; } = -0.01f;

    //Currect values
    private float shootDelay;
    private bool isReloading = false;

    public static Weapon Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;

        lidar = GetComponent<Lidar>();
        shootVariation = FindObjectOfType<ShootVariation>(); // FindObjectOfType imao
        weaponShake = GetComponentInChildren<CinemachineImpulseSource>();

        SpeedMultiplier = Config.SpeedMultiplier;
        WeaponDamage = Config.WeaponDamage;
        WeaponShootDelay = Config.WeaponShootDelay;
        WeaponAccuracy = Config.WeaponAccuracy;
        WeaponAmmo = Config.WeaponAmmo;
        WeaponReloadTime = Config.WeaponReloadTime;

        CurrectWeaponAmmo = WeaponAmmo;
    }

    public void Update()
    {
        if (!Controller.CanMove) return;
        lidar.enabled = !IsHoldingWeapon & Delay <= 0;

        if (Input.GetKeyDown(KeyCode.Q) & Delay <= 0)
        {
            Delay = delaySet / SpeedMultiplier;
            IsHoldingWeapon = !IsHoldingWeapon;
        }
        if (Delay > 0) Delay -= Time.deltaTime;

        if (Input.GetMouseButton(0) && Delay <= 0 && IsHoldingWeapon && shootDelay <= 0 && CurrectWeaponAmmo > 0 && !isReloading)
        {
            shootDelay = WeaponShootDelay;
            CurrectWeaponAmmo -= 1;
            CameraShakeManager.instance.Shake(weaponShake, WeaponDamage * shakePower);
            shootDelay += shootVariation.Shoot(shootPosition, shootDirection);
        }
        if (shootDelay > 0) shootDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurrectWeaponReloadTime <= -0.01f)
            {
                CurrectWeaponReloadTime = (WeaponReloadTime + AdditionalReloadTime) / SpeedMultiplier;
                isReloading = true;
            }
        }
        if (CurrectWeaponReloadTime <= -0.01f & isReloading)
        {
            CurrectWeaponAmmo = WeaponAmmo;
            isReloading = false;
        }
        if (CurrectWeaponReloadTime > -0.01f) CurrectWeaponReloadTime -= Time.deltaTime;
    }
}
