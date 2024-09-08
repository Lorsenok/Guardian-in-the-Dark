using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubArea : MonoBehaviour
{
    public static GameObject Instance { get; private set; }

    public static bool IsOnPlayer { get; private set; } = false;

    private float curHP;
    private bool isEnemyAlive = false;

    private void Awake()
    {
        Instance = gameObject;
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemySpawned += OnEnemySpawn;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemySpawned -= OnEnemySpawn;
    }

    private void OnEnemySpawn()
    {
        isEnemyAlive = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>())
        {
            IsOnPlayer = true;
            curHP = PlayerManager.Instance.HP;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>())
        {
            IsOnPlayer = false;
        }
    }

    private void Update()
    {
        if (EnemyManager.LastEnemy == null) isEnemyAlive = false;

        if (IsOnPlayer)
        {
            PlayerManager.Instance.HP = curHP;

            if (isEnemyAlive)
            {
                EnemyManager.LastEnemy.Die(false);
                isEnemyAlive = false;
            }
        }
    }
}
