using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject weaponReloadBar;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadText;

    private float startHP;

    private Weapon weapon;

    private void Start()
    {
        startHP = PlayerManager.Instance.HP;
    }

    private void LateUpdate()
    {
        healthBar.transform.localScale = new Vector2(1 / startHP * PlayerManager.Instance.HP, healthBar.transform.localScale.y);

        if (weapon == null) weapon = Weapon.Instance;
        if (weapon != null)
        {
            ammoText.text = "Ammo: " + weapon.CurrectWeaponAmmo.ToString();
            weaponReloadBar.transform.localScale = new Vector2(1 - 1 / (weapon.WeaponReloadTime + weapon.AdditionalReloadTime) * weapon.CurrectWeaponReloadTime, weaponReloadBar.transform.localScale.y);
            if (weapon.CurrectWeaponAmmo <= 0 && weapon.CurrectWeaponReloadTime <= -0.01f) 
                reloadText.color = Color.Lerp(reloadText.color, new Color(reloadText.color.r, reloadText.color.g, reloadText.color.b, 1), Time.deltaTime * 2);
            else reloadText.color = Color.Lerp(reloadText.color, new Color(reloadText.color.r, reloadText.color.g, reloadText.color.b, 0), Time.deltaTime * 2);
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
