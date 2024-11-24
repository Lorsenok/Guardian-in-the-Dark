using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public static ShootVariation shootVariation;

    public bool IsHoldingWeapon { get; private set; } = false;
    public bool AllowSwitching { get; set; } = true;

    public Lidar Lidar { get; private set; }

    [SerializeField] private AudioSource sound;

    [SerializeField] private float accuracy;
    [SerializeField] private float spreadDecrease;
    [SerializeField] private float minSpread;
    [SerializeField] private float maxSpread;

    [SerializeField] private float delaySet;
    public float Delay { get; set; }

    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform shootDirection;

    [SerializeField] private float shakePower;

    [SerializeField] private GameObject lidar3DModel;
    [SerializeField] private GameObject weapon3DModel;

    [SerializeField] private Transform weaponStartPoint;
    [SerializeField] private float weaponWidth;
    [SerializeField] private LayerMask bordersLayerMask;

    public CinemachineImpulseSource WeaponShake { get; private set; }

    public float SpeedMultiplier { get; set; }
    public float ShootSpeedMultiplier { get; set; }
    public float ReloadSpeedMultiplier { get; set; }
    public float WeaponDamage { get; set; }
    public float WeaponShootDelay { get; set; }
    public int WeaponAmmo { get; set; }
    public float CurrectWeaponAmmo { get; set; }
    public float WeaponReloadTime { get; set; }
    public float AdditionalReloadTime { get; set; } = 0;
    public float CurrectWeaponReloadTime { get; set; } = -0.01f;

    public float CurrectSpread { get; private set; } = 0f;
    public float LastSpread { get; private set; } = 0f;

    private float shootDelay;
    private bool isReloading = false;

    public static Weapon Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        Lidar = GetComponent<Lidar>();
        shootVariation = FindObjectOfType<ShootVariation>(); // FindObjectOfType imao
        WeaponShake = GetComponentInChildren<CinemachineImpulseSource>();

        SpeedMultiplier = Config.SpeedMultiplier;
        ShootSpeedMultiplier = Config.ShootSpeedMultiplier;
        ReloadSpeedMultiplier = Config.ReloadSpeedMultiplier;
        WeaponDamage = Config.WeaponDamage;
        WeaponShootDelay = Config.WeaponShootDelay;
        WeaponAmmo = Config.WeaponAmmo;
        WeaponReloadTime = Config.WeaponReloadTime;

        CurrectWeaponAmmo = WeaponAmmo;
    }

    private bool switchOnce = false;
    public void Update()
    {
        if (!Controller.CanMove)
        {
            lidar3DModel.SetActive(false);
            weapon3DModel.SetActive(false);
            return;
        }
        Lidar.IsWorking = !IsHoldingWeapon & Delay <= 0;
        lidar3DModel.SetActive(Lidar.IsWorking);

        if (Input.GetKeyDown(KeyCode.Q) & Delay <= 0 && PlayerManager.Instance.IsMenuClosed && AllowSwitching)
        {
            Delay = delaySet;
            switchOnce = true;
        }
        if (Delay > 0) Delay -= Time.deltaTime * SpeedMultiplier;

        if (Delay <= 0 && switchOnce)
        {
            IsHoldingWeapon = !IsHoldingWeapon;
            switchOnce = false;
        }

        if (Delay <= 0 && IsHoldingWeapon)
        {
            weapon3DModel.SetActive(true);

            Vector2 borderPos = Physics2D.Raycast(weaponStartPoint.position, -(transform.position - weaponStartPoint.position), weaponWidth, bordersLayerMask).point;
            Vector3 curEulerAngle;

            if (Vector2.Distance(borderPos, weaponStartPoint.position) <= weaponWidth && borderPos != Vector2.zero)
                curEulerAngle = new Vector3(0, 0, 90f - 90f / weaponWidth * Vector2.Distance(borderPos, weaponStartPoint.position));
            else curEulerAngle = Vector3.zero;

            weaponStartPoint.localEulerAngles = Vector3.Lerp(weaponStartPoint.localEulerAngles, curEulerAngle, Time.deltaTime * 10);

            if (Input.GetMouseButton(0) && shootDelay <= 0 && CurrectWeaponAmmo > 0 && !isReloading && PlayerManager.Instance.IsMenuClosed)
            {
                shootDelay = WeaponShootDelay;
                CurrectWeaponAmmo -= 1;
                CameraShakeManager.Instance.Shake(WeaponShake, WeaponDamage * shakePower);
                shootDelay += shootVariation.Shoot(shootPosition, shootDirection);
                LastSpread = CurrectSpread;
                CurrectSpread += accuracy;

                sound.volume = Config.Sound;
                sound.Play();
            }
        }
        else
        {
            weapon3DModel.SetActive(false);
        }
        if (shootDelay > 0) shootDelay -= Time.deltaTime * ShootSpeedMultiplier;

        if (Input.GetKeyDown(KeyCode.R) && PlayerManager.Instance.IsMenuClosed)
        {
            CurrectSpread = minSpread;
            if (CurrectWeaponReloadTime <= -0.01f)
            {
                CurrectWeaponReloadTime = (WeaponReloadTime + AdditionalReloadTime);
                isReloading = true;
            }
        }
        if (CurrectWeaponReloadTime <= -0.01f & isReloading)
        {
            CurrectWeaponAmmo = WeaponAmmo;
            isReloading = false;
        }
        if (CurrectWeaponReloadTime > -0.01f) CurrectWeaponReloadTime -= Time.deltaTime * ReloadSpeedMultiplier;

        CurrectSpread -= Time.deltaTime * spreadDecrease;
        CurrectSpread = Mathf.Clamp(CurrectSpread, minSpread, maxSpread);
    }
}
