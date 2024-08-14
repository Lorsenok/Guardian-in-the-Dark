using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubArea : MonoBehaviour
{
    private float curHP;
    private bool onPlayer = false;
    private bool isEnemyAlive = false;

    private void OnEnable()
    {
        EnemyManager.OnEnemySpawned += OnEnemySpawn;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemySpawned -= OnEnemySpawn;
    }

    public void OnEnemySpawn()
    {
        isEnemyAlive = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>())
        {
            onPlayer = true;
            curHP = PlayerManager.Instance.HP;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>())
        {
            onPlayer = false;
        }
    }

    private void Update()
    {
        if (onPlayer)
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
