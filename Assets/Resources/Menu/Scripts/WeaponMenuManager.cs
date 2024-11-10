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
    }

    private void Update()
    {
        text.text = "Money: " + Config.Money.ToString();

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
    }
}
