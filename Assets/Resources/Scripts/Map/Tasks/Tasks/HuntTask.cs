using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HuntTask : Task
{
    public static bool IsEnemiesExist { get; set; }

    public int EnemiesCount { get; set; }

    private void OnEnemyDie()
    {
        EnemiesCount--;
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += OnEnemyDie;
    }

    public override bool Check()
    {
        return EnemiesCount <= 0;
    }

    private void Start()
    {
        if (!IsEnemiesExist)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Name = "Kill " + EnemiesCount.ToString() + (EnemiesCount == 1 ? "enemy" : "enemies");
    }
}
