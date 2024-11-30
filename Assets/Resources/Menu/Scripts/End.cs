using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cheatText;

    private void Update()
    {
        cheatText.enabled = Config.HasUsedCheats;
    }
}
