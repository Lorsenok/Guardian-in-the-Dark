using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

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

    [SerializeField] private GameObject lidar3DModel;
    [SerializeField] private GameObject weapon3DModel;

    [SerializeField] private Transform weaponStartPoint;
    [SerializeField] private float weaponWidth;
    [SerializeField] private LayerMask bordersLayerMask;

    public CinemachineImpulseSource WeaponShake { get; private set; }

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

    private void Awake()
    {
        if (Instance == null) Instance = this;

        lidar = GetComponent<Lidar>();
        shootVariation = FindObjectOfType<ShootVariation>(); // FindObjectOfType imao
        WeaponShake = GetComponentInChildren<CinemachineImpulseSource>();

        SpeedMultiplier = Config.SpeedMultiplier;
        WeaponDamage = Config.WeaponDamage;
        WeaponShootDelay = Config.WeaponShootDelay;
        WeaponAccuracy = Config.WeaponAccuracy;
        WeaponAmmo = Config.WeaponAmmo;
        WeaponReloadTime = Config.WeaponReloadTime;

        CurrectWeaponAmmo = WeaponAmmo;
    }

    private bool switchOnce = false;
    public void Update()
    {
        if (!Controller.CanMove) return;
        lidar.IsWorking = !IsHoldingWeapon & Delay <= 0;
        lidar3DModel.SetActive(lidar.IsWorking);

        if (Input.GetKeyDown(KeyCode.Q) & Delay <= 0)
        {
            Delay = delaySet / SpeedMultiplier;
            switchOnce = true;
        }
        if (Delay > 0) Delay -= Time.deltaTime;

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

            if (Input.GetMouseButton(0) && shootDelay <= 0 && CurrectWeaponAmmo > 0 && !isReloading)
            {
                shootDelay = WeaponShootDelay;
                CurrectWeaponAmmo -= 1;
                CameraShakeManager.instance.Shake(WeaponShake, WeaponDamage * shakePower);
                shootDelay += shootVariation.Shoot(shootPosition, shootDirection);
            }
        }
        else
        {
            weapon3DModel.SetActive(false);
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
