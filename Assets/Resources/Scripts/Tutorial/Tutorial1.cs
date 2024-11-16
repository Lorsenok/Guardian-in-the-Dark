using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    private float alpha = 1f;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI spr;

    private Color startColor;

    private bool isWorking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition())
        {
            isWorking = true;
        }
    }

    private void Start()
    {
        startColor = spr.color;
    }

    private void Update()
    {
        Weapon.Instance.Lidar.IsWorking = isWorking;

        if (!isWorking)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            alpha -= Time.deltaTime * speed;
        }

        spr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (alpha < 0f)
        {
            alpha = 0f;
            enabled = false;
        }
    }
}
