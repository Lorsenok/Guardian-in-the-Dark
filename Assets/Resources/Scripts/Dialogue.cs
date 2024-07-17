using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public bool IsWorking = false;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] [TextArea] string[] dialogueTexts;
    [SerializeField] private float symbolsShowDelay;
    [SerializeField] private float textsShowDelay;
    [SerializeField] private bool isLooping;

    private int curText = 0;
    private int curSymbol = 0;

    private float curSymbolDelay = 0;
    private float curTextsShowDelay = 0;

    private void Start()
    {
        curTextsShowDelay = textsShowDelay;
    }

    private void Update()
    {
        if (!IsWorking) return;

        if (curText == dialogueTexts.Length)
        {
            if (!isLooping)
            {
                IsWorking = false;
                return;
            }
            else curText = 0;
        }

        if (dialogueTexts[curText].Length == curSymbol)
        {
            if (curTextsShowDelay <= 0)
            {
                text.text = string.Empty;
                curSymbol = 0;
                curText++;
                curTextsShowDelay = textsShowDelay;
            }
            else curTextsShowDelay -= Time.deltaTime;

            return;
        }

        if (curSymbolDelay <= 0)
        {
            text.text += dialogueTexts[curText][curSymbol];
            curSymbolDelay = symbolsShowDelay;
            curSymbol++;
        }
        else curSymbolDelay -= Time.deltaTime;
    }
}