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
        if (Config.CurrectCompleteAward == 0)
        {
            Destroy(gameObject);
            return;
        }

        GameButton.CanBeToched = false;
    }

    private void Update()
    {
        textObj.text = text + Config.CurrectCompleteAward.ToString();

        if (Input.GetMouseButton(0))
        {
            GameButton.CanBeToched = true;
            Config.Money += Config.CurrectCompleteAward;
            PlayerPrefs.SetInt("money", Config.Money);
            Destroy(gameObject);
        }
    }
}
