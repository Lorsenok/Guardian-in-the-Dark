using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    // Game settings
    public static float ShakePower { get; set; } = 1f;
    public static int DifficulityMultiplier { get; set; } = 1;

    // Map settings
    public static int MapSize { get; set; } = 1;
    public static float EnemySpawnRate { get; set; } = 20f;
    public static float EnemySpawnRateBias { get; set; } = 10f;
    public static GameObject[] EnemyManagers { get; set; }
    public static int CurrectCompleteAward { get; set; } = 1;
    public static bool HasEnd { get; set; } = false;
    public static int EndSceneID { get; set; } = 0;

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



    public static int Money { get; set; } = 10;
    public static int StartMoney { get; set; } = 10;
    public static int Progress { get; set; } = 0;


    public static bool HasUsedCheats { get; set; } = false;


    //Settings
    public static float Sound { get; set; } = 1.0f;
    public static bool LidarPointSounds { get; set; } = true;
    public static float Music { get; set; } = 1.0f;

    public static bool CameraShake { get; set; } = true;
    public static bool Particles { get; set; } = true;

    public static float PostProcessingPower { get; set; } = 1.0f;

    public static bool AnalogGlitchEffect { get; set; } = true;
    public static bool FisheyeEffect { get; set; } = true;

    public static int Pixelization { get; set; } = 180;


    //Default Settings
    public static float SoundDefault { get; set; } = 1.0f;
    public static bool LidarPointSoundsDefault { get; set; } = true;
    public static float MusicDefault { get; set; } = 1.0f;

    public static bool CameraShakeDefault { get; set; } = true;
    public static bool ParticlesDefault { get; set; } = true;

    public static float PostProcessingPowerDefault { get; set; } = 1.0f;

    public static bool AnalogGlitchEffectDefault { get; set; } = true;
    public static bool FisheyeEffectDefault { get; set; } = true;

    public static int PixelizationDefault { get; set; } = 180; 
}
