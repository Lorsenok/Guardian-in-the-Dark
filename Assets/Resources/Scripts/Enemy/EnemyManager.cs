using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private int spawnChance;
    [SerializeField] private float spawnTimeM = 1f;

    [SerializeField] private float additionalHPLose = 2.5f;

    public static Action OnEnemySpawned;

    [SerializeField] private float spawnTime;

    [Header("Post Processing")]
    [SerializeField] private float changeSpeed;
    [SerializeField] private float vignetteSet;
    [SerializeField] private Color vignetteColor;
    [SerializeField] private float chromaticAberrationSet;
    [SerializeField] private float bloomSet = 100;
    [SerializeField] private float digitalGlitchSet;
    [SerializeField] private float analogGlitchSet = 0.45f;

    [Header("Shake")]
    [SerializeField] private float shakeIntensity;

    private Lidar lidar;

    public bool IsEnemyAlive { get; set; } = false;

    private bool isEnemyOur = false;

    public static Enemy LastEnemy { get; private set; }

    public int SpawnChance { get; set; }

    private CinemachineImpulseSource shake;

    public void OnEnemySpawn()
    {
        IsEnemyAlive = true;
    }

    public void OnEnemyDestroyed()
    {
        IsEnemyAlive = false;
        isEnemyOur = false;
        PlayerManager.Instance.AdditionalHPLossSpeed = 0;
    }

    private void SpawnTimeExpand()
    {
        spawnTime += Config.EnemySpawnRate * spawnTimeM;
    }

    private void SpawnEnemy()
    {
        if (lidar.IsWorking && lidar.TimeSinceLastRay < 1f)
        {
            Vector2 position = (player.position + new Vector3(lidar.CurrectRay.point.x, lidar.CurrectRay.point.y, 0)) / 2;

            if (Input.GetMouseButton(0) && Vector2.Distance(position, player.position) >= minDistance && Vector2.Distance(position, player.position) <= maxDistance)
            {
                Instantiate(enemy, position, Quaternion.identity).TryGetComponent(out Enemy lastEnemy);
                LastEnemy = lastEnemy;
                if (LastEnemy != null) LastEnemy.Em = this;
                SpawnTimeExpand();
                OnEnemySpawned -= OnEnemySpawn;
                OnEnemySpawned?.Invoke();
                OnEnemySpawned += OnEnemySpawn;
                IsEnemyAlive = true;
                isEnemyOur = true;
            }
        }
    }

    private void Awake()
    {
        OnEnemySpawned += OnEnemySpawn;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyed;

        SpawnChance = spawnChance;
        SpawnTimeExpand();
    }

    private void Start()
    {
        shake = Weapon.Instance.WeaponShake;
    }

    private Transform player;

    private void LateUpdate()
    {
        if (player == null & PlayerManager.Instance.GetPlayerPosition() == null) return;
        else if (player == null)
        {
            player = PlayerManager.Instance.GetPlayerPosition();
            lidar = player.gameObject.GetComponent<Lidar>();
        }

        if (!HubArea.IsOnPlayer)
        {
            if (spawnTime > 0 & !IsEnemyAlive) spawnTime -= Time.deltaTime;
            else if (!IsEnemyAlive) SpawnEnemy();
        }

        if (IsEnemyAlive && isEnemyOur)
        {
            PlayerManager.Instance.AdditionalHPLossSpeed = additionalHPLose;
            PostProcessingController.Instance.VignetteSet(vignetteSet, vignetteColor, changeSpeed);
            PostProcessingController.Instance.ChromaticAberrationSet(chromaticAberrationSet, changeSpeed);
            PostProcessingController.Instance.BloomSet(bloomSet, changeSpeed);
            PostProcessingController.Instance.DigitalGlitchSet(digitalGlitchSet, changeSpeed);
            PostProcessingController.Instance.AnalogGlitchSet(analogGlitchSet, changeSpeed);
            CameraShakeManager.Instance.Shake(shake, shakeIntensity);
        }
    }
}
