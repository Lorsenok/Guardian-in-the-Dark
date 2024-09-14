using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        text.text = Config.Money.ToString();
    }
}
