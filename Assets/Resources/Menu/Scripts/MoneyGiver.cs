using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyGiver : MonoBehaviour
{
    [SerializeField] private int money;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        GameButton.CanBeToched = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GameButton.CanBeToched = true;
            Config.Money += money;
            Destroy(gameObject);
        }
    }
}
