using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficulityButton : GameButton
{
    private static bool canBePicked = true;

    [Header("Diff Setup")]
    [SerializeField] private int award;
    [SerializeField] private int mapSize;
    [SerializeField] private GameObject[] enemyManagersPull;
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private float enemySpawnRateBias;
    [SerializeField] private int difficulityMultiplier;
    [SerializeField] private float hp;
    [SerializeField] private int progressSet;
    [SerializeField] private TextMeshProUGUI text;

    public override void Awake()
    {
        if (PlayerPrefs.HasKey("prog")) Config.Progress = PlayerPrefs.GetInt("prog");

        text.gameObject.SetActive(Config.Progress < progressSet - 1);

        base.Awake();
    }

    public override void Update()
    {
        if (Config.Progress < progressSet - 1 || !canBePicked) return;

        base.Update();

        if (isMousePointing && Input.GetMouseButtonUp(mouseButton))
        {
            Config.CurrectCompleteAward = award;
            Config.MapSize = mapSize;
            Config.EnemyManagers = enemyManagersPull;
            Config.EnemySpawnRate = enemySpawnRate;
            Config.EnemySpawnRateBias = enemySpawnRateBias;
            Config.DifficulityMultiplier = difficulityMultiplier;
            Config.PlayerHP = hp;

            if (progressSet > Config.Progress)
            {
                Config.Progress = progressSet;
                PlayerPrefs.SetInt("prog", progressSet);
            }

            canBePicked = false;
        }
    }
}
