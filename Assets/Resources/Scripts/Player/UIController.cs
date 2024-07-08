using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerManager pm;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject weaponReloadBar;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadText;

    private float startHP;

    private Weapon weapon;

    private void Start()
    {
        startHP = pm.HP;
    }

    private void Update()
    {
        healthBar.transform.localScale = new Vector2(1 / startHP * pm.HP, healthBar.transform.localScale.y);

        if (weapon == null) weapon = Weapon.Instance;
        if (weapon != null)
        {
            ammoText.text = "Ammo: " + weapon.CurrectWeaponAmmo.ToString();
            weaponReloadBar.transform.localScale = new Vector2(1 - 1 / (weapon.WeaponReloadTime + weapon.AdditionalReloadTime) * weapon.CurrectWeaponReloadTime, weaponReloadBar.transform.localScale.y);
            if (weapon.CurrectWeaponAmmo <= 0 && weapon.CurrectWeaponReloadTime <= -0.01f) reloadText.text = "Press R to reload";
            else reloadText.text = string.Empty;
        }

        if (!weapon.IsHoldingWeapon)
        {
            ammoText.text = "Lidar";
        }

        if (weapon.Delay > 0)
        {
            ammoText.color = new Color(ammoText.color.r, ammoText.color.g, ammoText.color.b, 0.5f);
        }

        else
        {
            ammoText.color = new Color(ammoText.color.r, ammoText.color.g, ammoText.color.b, 1);
        }
    }
}
