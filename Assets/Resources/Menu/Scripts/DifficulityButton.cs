using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficulityButton : GameButton
{
    [Header("Diff Setup")]
    [SerializeField] private int mapSize;
    [SerializeField] private GameObject[] enemyManagersPull;
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private int difficulityMultiplier;
    [SerializeField] private float hp;

    public override void Update()
    {
        base.Update();

        if (isMousePointing && Input.GetMouseButtonUp(mouseButton))
        {
            Config.MapSize = mapSize;
            Config.EnemyManagers = enemyManagersPull;
            Config.EnemySpawnRate = enemySpawnRate;
            Config.DifficulityMultiplier = difficulityMultiplier;
            Config.PlayerHP = hp;
        }
    }
}
