using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial4 : MonoBehaviour
{
    private float alpha = 1f;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI spr;

    private Color startColor;

    private void Start()
    {
        Generator.OnUse += OnGeneratorUsed;
        startColor = spr.color;
    }

    [SerializeField] private int count;

    private void OnGeneratorUsed()
    {
        count--;
    }

    private void Update()
    {
        if (count > 0)
        {
            return;
        }

        alpha = Mathf.Lerp(alpha, 0f, Time.deltaTime * speed);
        spr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        if (alpha < 0f) alpha = 0f;
    }
}
