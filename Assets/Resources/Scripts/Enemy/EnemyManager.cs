using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField, Range(0.001f, 1f)] private float spawnChance;

    public static Action OnEnemySpawned;

    [SerializeField] private float spawnTime;

    [Header("Post Processing")]
    [SerializeField] private float vignetteSet;
    [SerializeField] private float changeSpeed;
    [SerializeField] private Color vignetteColor;

    [Header("Shake")]
    [SerializeField] private float shakeIntensity;

    private Lidar lidar;

    public bool IsEnemyAlive { get; set; } = false;

    private CinemachineImpulseSource shake;

    public void OnEnemySpawn()
    {
        spawnTime += (Config.EnemySpawnRate / spawnChance) / 2;
    }

    public void OnEnemyDestroyed()
    {
        IsEnemyAlive = false;
    }

    private void SpawnEnemy()
    {
        if (lidar.IsWorking && lidar.TimeSinceLastRay < 1f)
        {
            Vector2 position = (player.position + new Vector3(lidar.CurrectRay.point.x, lidar.CurrectRay.point.y, 0)) / 2;

            if (Input.GetMouseButton(0) && Vector2.Distance(position, player.position) >= minDistance && Vector2.Distance(position, player.position) <= maxDistance)
            {
                Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>().em = this;
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
            PostProcessingController.Instance.VignetteSet(vignetteSet, vignetteColor, changeSpeed);
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
