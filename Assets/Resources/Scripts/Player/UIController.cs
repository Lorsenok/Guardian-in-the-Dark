using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image weaponReloadBar;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Color startAmmoTextColor;
    [SerializeField] private Color endAmmoTextColor;

    [SerializeField] private float appearSpeed;

    [SerializeField] private Image bombReloadBar;
    [SerializeField] private float bombReloadBarSize;

    private Weapon weapon;

    private void LateUpdate()
    {
        if (weapon == null) weapon = Weapon.Instance;
        if (weapon != null)
        {
            ammoText.text = weapon.CurrectWeaponAmmo.ToString() + "/" + Config.WeaponAmmo;
            weaponReloadBar.transform.localScale = new Vector2(1 - 1 / (weapon.WeaponReloadTime + weapon.AdditionalReloadTime) * weapon.CurrectWeaponReloadTime, weaponReloadBar.transform.localScale.y);
        }


        weaponReloadBar.color = Color.Lerp(weaponReloadBar.color,
            new Color(weaponReloadBar.color.r, weaponReloadBar.color.g, weaponReloadBar.color.b, weapon.CurrectWeaponReloadTime > 0 & weapon.IsHoldingWeapon ? 1f : 0f), 
            Time.deltaTime * appearSpeed);

        ammoText.color = Color.Lerp(ammoText.color, 
            (weapon.CurrectWeaponAmmo > 0 ? startAmmoTextColor : endAmmoTextColor) * new Color(1, 1, 1, weapon.IsHoldingWeapon ? 1f : 0f), 
            Time.deltaTime * appearSpeed);

        bombReloadBar.transform.localScale = new Vector3(bombReloadBar.transform.localScale.x, bombReloadBarSize / weapon.Lidar.BombSpawnDelay * weapon.Lidar.CurBombSpawnDelay);
        bombReloadBar.color = Color.Lerp(bombReloadBar.color,
            new Color(bombReloadBar.color.r, bombReloadBar.color.g, bombReloadBar.color.b, weapon.Lidar.CurBombSpawnDelay > 0 & !weapon.IsHoldingWeapon ? 1f : 0f),
            Time.deltaTime * appearSpeed);

    }
}
