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
    private float delay;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform shootDirection;

    [SerializeField] private float shakePower;

    private CinemachineImpulseSource weaponShake;

    //Config
    public float SpeedMultiplier { get; set; }
    public float WeaponDamage { get; set; }
    public float WeaponShootDelay { get; set; }
    public float WeaponAccuracy { get; set; }
    public int WeaponAmmo { get; set; }
    public float WeaponReloadTime { get; set; }

    //Currect values
    private float currectWeaponAmmo;
    private float shootDelay;
    private float reload;
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

        currectWeaponAmmo = WeaponAmmo;
    }

    public void Update()
    {
        if (!Controller.CanMove) return;
        lidar.enabled = !IsHoldingWeapon & delay <= 0;

        if (Input.GetKeyDown(KeyCode.Q) & delay <= 0)
        {
            delay = delaySet / SpeedMultiplier;
            IsHoldingWeapon = !IsHoldingWeapon;
        }
        if (delay > 0) delay -= Time.deltaTime;

        if (Input.GetMouseButton(0) && delay <= 0 && IsHoldingWeapon && shootDelay <= 0 && currectWeaponAmmo > 0 && !isReloading)
        {
            shootDelay = WeaponShootDelay;
            currectWeaponAmmo -= 1;
            CameraShakeManager.instance.Shake(weaponShake, WeaponDamage * shakePower);
            shootDelay += shootVariation.Shoot(shootPosition, shootDirection);
        }
        if (shootDelay > 0) shootDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (reload <= 0)
            {
                reload = WeaponReloadTime / SpeedMultiplier;
                isReloading = true;
            }
        }
        if (reload <= 0 & isReloading)
        {
            currectWeaponAmmo = WeaponAmmo;
            isReloading = false;
        }
        if (reload > 0) reload -= Time.deltaTime;
    }
}
