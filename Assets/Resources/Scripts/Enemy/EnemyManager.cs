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
    [SerializeField, Range(0.001f, 1f)] private float spawnChance;

    [SerializeField] private float additionalHPLose = 2.5f;

    public static Action OnEnemySpawned;

    [SerializeField] private float spawnTime;

    [Header("Post Processing")]
    [SerializeField] private float changeSpeed;
    [SerializeField] private float vignetteSet;
    [SerializeField] private Color vignetteColor;
    [SerializeField] private float chromaticAberrationSet;
    [SerializeField] private float bloomSet = 100;

    [Header("Shake")]
    [SerializeField] private float shakeIntensity;

    private Lidar lidar;

    public bool IsEnemyAlive { get; set; } = false;

    public static Enemy LastEnemy { get; private set; }

    private CinemachineImpulseSource shake;

    public void OnEnemySpawn()
    {
        IsEnemyAlive = true;
        spawnTime += (Config.EnemySpawnRate / spawnChance) / 2;
    }

    public void OnEnemyDestroyed()
    {
        IsEnemyAlive = false;
        PlayerManager.Instance.AdditionalHPLossSpeed = 0;
    }

    private void SpawnEnemy()
    {
        if (lidar.IsWorking && lidar.TimeSinceLastRay < 1f)
        {
            Vector2 position = (player.position + new Vector3(lidar.CurrectRay.point.x, lidar.CurrectRay.point.y, 0)) / 2;

            if (Input.GetMouseButton(0) && Vector2.Distance(position, player.position) >= minDistance && Vector2.Distance(position, player.position) <= maxDistance)
            {
                LastEnemy = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
                LastEnemy.em = this;
                spawnTime += Config.EnemySpawnRate / spawnChance;
                OnEnemySpawned -= OnEnemySpawn;
                OnEnemySpawned?.Invoke();
                OnEnemySpawned += OnEnemySpawn;
                IsEnemyAlive = true;
            }
        }
    }

    private void Start()
    {
        shake = Weapon.Instance.WeaponShake;
        spawnTime += Config.EnemySpawnRate / spawnChance;
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

        if (spawnTime > 0 & !IsEnemyAlive) spawnTime -= Time.deltaTime;
        else if (!IsEnemyAlive) SpawnEnemy();

        if (IsEnemyAlive)
        {
            PlayerManager.Instance.AdditionalHPLossSpeed = additionalHPLose;
            PostProcessingController.Instance.VignetteSet(vignetteSet, vignetteColor, changeSpeed);
            PostProcessingController.Instance.ChromaticAberrationSet(chromaticAberrationSet, changeSpeed);
            PostProcessingController.Instance.BloomSet(bloomSet, changeSpeed);
            CameraShakeManager.instance.Shake(shake, shakeIntensity);
        }
    }

    private void OnEnable()
    {
        OnEnemySpawned += OnEnemySpawn;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        OnEnemySpawned -= OnEnemySpawn;
        Enemy.OnEnemyDestroyed -= OnEnemyDestroyed;
    }

}
