using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyGiver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;
    [SerializeField] private string text;

    private void Start()
    {
        GameButton.CanBeToched = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            textObj.text = text + Config.CurrectCompleteAward.ToString();
            GameButton.CanBeToched = true;
            Config.Money += Config.CurrectCompleteAward;
            Destroy(gameObject);
        }
    }
}
