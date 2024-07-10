using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float minDistance;
    [SerializeField, Range(0.001f, 1f)] private float spawnChance;

    public static Action OnEnemySpawned;

    [SerializeField] private float spawnTime;

    private Lidar lidar;

    private bool isEnemyAlive = false;

    private void OnEnemySpawn()
    {
        spawnTime += (Config.EnemySpawnRate / spawnChance) / 2;
    }

    private void OnEnemyDestroyed()
    {
        isEnemyAlive = false;
    }

    private void SpawnEnemy()
    {
        if (lidar.enabled)
        {
            Vector2 position = (player.position + new Vector3(lidar.CurrectRay.point.x, lidar.CurrectRay.point.y, 0)) / 2;

            if (Input.GetMouseButton(0) & Vector2.Distance(position, player.position) > minDistance)
            {
                Instantiate(enemy, position, Quaternion.identity);
                spawnTime += Config.EnemySpawnRate / spawnChance;
                OnEnemySpawned -= OnEnemySpawn;
                OnEnemySpawned?.Invoke();
                OnEnemySpawned += OnEnemySpawn;
                isEnemyAlive = true;
            }
        }
    }

    private void Start()
    {
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

        if (spawnTime > 0 & !isEnemyAlive) spawnTime -= Time.deltaTime;
        else if (!isEnemyAlive) SpawnEnemy();
    }

    private void OnEnable()
    {
        OnEnemySpawned += OnEnemySpawn;
        Enemy.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        OnEnemySpawned -= OnEnemySpawn;
    }

}
