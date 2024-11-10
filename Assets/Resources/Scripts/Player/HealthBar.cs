using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform textCanvas;
    [SerializeField] private Transform line;

    public override void Start()
    {
        Count = PlayerManager.Instance.HP;
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        Count = PlayerManager.Instance.HP;
        Active = Input.GetKey(KeyCode.Tab);

        text.text = Mathf.Round(100/startCount*Count).ToString() + "/100%";
    }
}
