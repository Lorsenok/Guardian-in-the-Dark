using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponUpgradeButton : GameButton
{
    [Header("Weapon Upgrade")]

    [SerializeField] private TextMeshProUGUI textUpg;
    [SerializeField] private TextMeshProUGUI textCoast;

    [SerializeField] private int saveID;

    [SerializeField] private int coast;
    [SerializeField] private int additionalCoastByUpgrades;
    [SerializeField] private int upgradesMaxAmount;

    private int upgradesCurAmount = 0;

    [SerializeField] private float speed;
    [SerializeField] private float weaponSpread;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float reloadSpeed;

    [SerializeField] private int lidarRaysInTheSameTime;
    [SerializeField] private float lidarBombsReloadSpeedMultiplier;

    private void Upgrade()
    {
        coast += additionalCoastByUpgrades;

        Config.SpeedMultiplier += speed;
        Config.WeaponSpread += weaponSpread;
        Config.ShootSpeedMultiplier += shootSpeed;
        Config.ReloadSpeedMultiplier += reloadSpeed;

        Config.RaysAtTheSameTime += lidarRaysInTheSameTime;
        Config.BombReloadSpeedMultiplier += lidarBombsReloadSpeedMultiplier;
    }

    public override void Awake()
    {
        base.Awake();

        if (PlayerPrefs.HasKey("money")) Config.Money = PlayerPrefs.GetInt("money");

        if (!PlayerPrefs.HasKey("upg_" + saveID)) return;

        upgradesCurAmount = PlayerPrefs.GetInt("upg_" + saveID);
        for (int i = 0; i != upgradesCurAmount; i++) Upgrade();
    }

    public override void Update()
    {
        base.Update();

        textUpg.text = upgradesCurAmount.ToString() + "/" + upgradesMaxAmount.ToString();
        textCoast.text = upgradesCurAmount == upgradesMaxAmount ? "Max" : coast.ToString();

        if (!(isMousePointing && Input.GetMouseButtonUp(mouseButton) && Config.Money > coast && upgradesCurAmount < upgradesMaxAmount)) return;

        Config.Money -= coast;
        upgradesCurAmount += 1;

        PlayerPrefs.SetInt("upg_" + saveID, upgradesCurAmount);
        PlayerPrefs.SetInt("money", Config.Money);

        Upgrade();
    }
}
