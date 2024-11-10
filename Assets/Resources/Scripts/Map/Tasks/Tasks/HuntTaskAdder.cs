using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HuntTaskAdder : MonoBehaviour
{
    [SerializeField] private int defaultEnemiesCount;

    private void Awake()
    {
        if (Config.DifficulityMultiplier <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        for (int i = 1; i < defaultEnemiesCount * Config.DifficulityMultiplier; i++)
        {
            HuntTask task = gameObject.AddComponent<HuntTask>();
            task.EnemiesCount = i;
        }
    }
}
