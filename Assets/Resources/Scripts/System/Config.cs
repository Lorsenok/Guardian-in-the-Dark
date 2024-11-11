using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    // Game settings
    public static float Volume { get; set; } = 1.0f;
    public static bool PostProcessing { get; set; } = true;
    public static float ShakePower { get; set; } = 1f;
    public static int DifficulityMultiplier { get; set; } = 1;

    // Map settings
    public static int MapSize { get; set; } = 1;
    public static float EnemySpawnRate { get; set; } = 20f;
    public static float EnemySpawnRateBias { get; set; } = 10f;
    public static GameObject[] EnemyManagers { get; set; }
    public static int CurrectCompleteAward { get; set; } = 1;

    //Player settings
    public static float PlayerHP { get; set; } = 12000f;

    //Lidar settings
    public static int RaysAtTheSameTime { get; set; } = 1;
    public static float BombReloadSpeedMultiplier { get; set; } = 1f;

    //Weapon settings
    public static float SpeedMultiplier { get; set; } = 1.0f;
    public static float ReloadSpeedMultiplier { get; set; } = 1.0f;
    public static float ShootSpeedMultiplier { get; set; } = 1.0f;
    public static int WeaponDamage { get; set; } = 1;
    public static float WeaponShootDelay { get; set; } = 0.25f;
    public static float WeaponSpread { get; set; } = 0.05f;
    public static int WeaponAmmo { get; set; } = 10;
    public static float WeaponReloadTime { get; set; } = 4f;



    public static int Money { get; set; } = 1000;
    public static int Progress { get; set; } = 0;
}
