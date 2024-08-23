using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    // Game settings
    public static float Volume { get; set; } = 1.0f;
    public static bool PostProcessing { get; set; } = true;
    public static float ShakePower { get; set; } = 1;

    // Map settings
    public static float MapSize { get; set; } = 1.0f;
    public static float EnemySpawnRate { get; set; } = 10f;
    public static List<GameObject> EnemyManagers { get; set; }

    //Weapon settings
    public static float SpeedMultiplier { get; set; } = 1.0f;
    public static int WeaponDamage { get; set; } = 1;
    public static float WeaponShootDelay { get; set; } = 0.25f;
    public static float WeaponSpread { get; set; } = 0.1f;
    public static int WeaponAmmo { get; set; } = 10;
    public static float WeaponReloadTime { get; set; } = 1.0f;



    public int Money { get; set; } = 0;
}