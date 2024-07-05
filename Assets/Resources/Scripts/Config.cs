using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    // Game settings
    public static float Volume { get; set; } = 1.0f;
    public static bool PostProcessing { get; set; } = true;

    // Map settings
    public static float MapSize { get; set; } = 1.0f;
    //here also will be task
    //here also will be enemies pull

    //Weapon settings
    public static float SpeedMultiplier { get; set; } = 1.0f;
    public static float WeaponDamage { get; set; } = 1.0f;
    public static float WeaponShootSpeed { get; set; } = 1.0f;
    public static float WeaponAccuracy { get; set; } = 1.0f;
    public static float WeaponAmmo { get; set; } = 1.0f;
    public static float WeaponReloadSpeed { get; set; } = 1.0f;



    public int Money { get; set; } = 0 ;
}
