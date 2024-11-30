using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Menu[] decoHideMenus;

    [SerializeField] private Transform[] deco;
    [SerializeField] private float decoHideX;

    private List<float> startDecoXPositions;
    private List<float> hideDecoXPositions;

    [SerializeField] private float decoReturnSpeed;

    private void Start()
    {
        startDecoXPositions = new List<float>();
        hideDecoXPositions = new List<float>();
        foreach (Transform t in deco)
        {
            startDecoXPositions.Add(t.position.x);
            hideDecoXPositions.Add(t.position.x + decoHideX);
        }

        Config.HasUsedCheats = PlayerPrefs.GetInt("cheats") == 1;
    }

    private void Update()
    {
        text.text = (Config.HasUsedCheats ? "(Cheated) " : "") + "Money: " + Config.Money.ToString();

        bool isMenuHiding = false;

        foreach (Menu menu in decoHideMenus)
        {
            if (!menu.Open) continue;

            isMenuHiding = true;

            for (int i = 0; i < deco.Length; i++)
            {
                deco[i].position = Vector3.Lerp(deco[i].position,
                    new Vector3(hideDecoXPositions[i], deco[i].position.y, deco[i].position.z),
                    decoReturnSpeed * Time.deltaTime);
            }
        }

        if (isMenuHiding) return;

        for (int i = 0; i < deco.Length; i++)
        {
            deco[i].position = Vector3.Lerp(deco[i].position, 
                new Vector3(startDecoXPositions[i], deco[i].position.y, deco[i].position.z), 
                decoReturnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.L) && !Config.HasUsedCheats)
        {
            Config.HasUsedCheats = true;
            PlayerPrefs.SetInt("cheats", 1);
            Config.Money = 9999;
            PlayerPrefs.SetInt("money", 9999);
        }
    }
}
