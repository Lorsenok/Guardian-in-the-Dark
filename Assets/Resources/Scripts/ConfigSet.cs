using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigSet : MonoBehaviour
{
    //DELETE THIS SHIT AFTER CREATING MENU xd

    [Header("Game Settings")]
    [SerializeField] float Volume = 1.0f;
    [SerializeField] bool PostProcessing = true;
    [SerializeField] float ShakePower = 1;

    // Map settings
    [SerializeField] float MapSize = 1.0f;
    [SerializeField] float EnemySpawnRate = 60f;
    [SerializeField] List<GameObject> EnemyManagers;

    //Weapon settings
    [SerializeField] float SpeedMultiplier = 1.0f;
    [SerializeField] int WeaponDamage = 1;
    [SerializeField] float WeaponShootDelay = 0.25f;
    [SerializeField] float WeaponSpread = 0.1f;
    [SerializeField] int WeaponAmmo = 10;
    [SerializeField] float WeaponReloadTime = 1.0f;

    private void Awake()
    {
        Config.Volume = Volume;
        Config.PostProcessing = PostProcessing;
        Config.ShakePower = ShakePower;
        Config.MapSize = MapSize;
        Config.EnemySpawnRate = EnemySpawnRate;
        Config.EnemyManagers = EnemyManagers;
        Config.SpeedMultiplier = SpeedMultiplier;
        Config.WeaponDamage = WeaponDamage;
        Config.WeaponShootDelay = WeaponShootDelay;
        Config.WeaponSpread = WeaponSpread;
        Config.WeaponAmmo = WeaponAmmo;
        Config.WeaponReloadTime = WeaponReloadTime;
    }

    public int Money { get; set; } = 0;

    /*
        _        _
       ( `-.__.-' )
        `-.    .-'
          \  /
           ||
           ||
          //\\
         //  \\
        ||    ||
        ||____||
        ||====||
         \\  //
          \\//
           ||
           ||
           ||
           ||
           ||
           ||
           ||
           ||
           [] 
    */
}
