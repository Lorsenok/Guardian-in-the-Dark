using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial2 : MonoBehaviour
{
    private float alpha = 1f;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI spr;
    [SerializeField] private GameObject[] disableObjects;

    private Color startColor;

    private bool isWorking = false;

    private void Start()
    {
        startColor = spr.color;
    }

    private void Update()
    {
        foreach (GameObject obj in disableObjects)
        {
            obj.SetActive(!isWorking);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isWorking = true;
        }

        if (!isWorking)
        {
            return;
        }

        alpha = Mathf.Lerp(alpha, 0f, Time.deltaTime * speed);

        spr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (alpha < 0f) alpha = 0f;
    }
}
