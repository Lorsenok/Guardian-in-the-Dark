using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDecoText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    [SerializeField] private CinemachineImpulseSource impulse;


    [Header("Text1")]
    [SerializeField, TextArea] private string decoText;

    [SerializeField] private float textOutputDelay;
    [SerializeField] private float stopOutputDelay;
    [SerializeField] private float stopOutputMaxTime;
    [SerializeField] private float endTime;


    [Header("Text2")]
    [SerializeField] private float dissapearColorSpeed;
    [SerializeField] private float sinTimeM;
    [SerializeField] private float additionalSinTime;

    private float curTextOutputDelay = 0;
    private float curStopOutputDelay = 0;
    private float curStopOutputTime = 0;

    private float curEndTime = 0;

    private int curSymbol = -1;

    private void Start()
    {
        text1.text = string.Empty;

        curTextOutputDelay = textOutputDelay;
        curStopOutputDelay = stopOutputDelay;
    }

    private void Update()
    {
        if (curSymbol == decoText.Length - 1)
        {
            curStopOutputTime = 0;
            curSymbol = -1;
            text1.text = string.Empty;
            curEndTime = endTime;

            impulse.GenerateImpulse();
        }

        if (curEndTime > 0)
        {
            text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, additionalSinTime + ProjMath.SinTime(sinTimeM));

            curEndTime -= Time.deltaTime;
            return;
        }

        text2.color = Color.Lerp(text2.color, new Color(text2.color.r, text2.color.g, text2.color.b, 0), Time.deltaTime * dissapearColorSpeed);

        if (curStopOutputTime > 0)
        {
            curStopOutputTime -= Time.deltaTime;
            return;
        }

        curStopOutputDelay -= Time.deltaTime;
        curTextOutputDelay -= Time.deltaTime;

        if (curTextOutputDelay <= 0)
        {
            curSymbol++;
            text1.text += decoText[curSymbol];
            curTextOutputDelay = Random.Range(0, textOutputDelay);
        }

        if (curStopOutputDelay <= 0)
        {
            curStopOutputDelay = Random.Range(stopOutputDelay/2, stopOutputDelay);
            curStopOutputTime = Random.Range(0, stopOutputMaxTime);
        }
    }
}
