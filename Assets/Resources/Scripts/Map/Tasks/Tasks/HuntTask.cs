using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntTask : Task
{
    [SerializeField] private int enemiesCount;

    private void OnEnemyDie()
    {
        enemiesCount--;
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += OnEnemyDie;
    }
    private void OnDisable()
    {
        Enemy.OnEnemyDied += OnEnemyDie;
    }

    public override bool Check()
    {
        return enemiesCount <= 0;
    }
}
