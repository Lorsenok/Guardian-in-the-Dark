using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public sealed class PCTask : Task
{
    [SerializeField] private GameObject pcPrefab;
    [SerializeField] private GameObject papersPrefab;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField, Range(1, 10)] private int codeLength;

    private static string code;
    private int curCodeSym = 0;

    private void AddSymbol()
    {
        if (curCodeSym != codeLength) curCodeSym += 1;
    }

    public static bool CheckCode(string _code)
    {
        return code == _code;
    }

    private void OnEnable()
    {
        char[] simbols = new char[codeLength];

        for (int i = 0; i < codeLength; i++)
        {
            simbols[i] = Mathf.Round(Random.Range(0, 9)).ToString().ToCharArray()[0];
        }

        code = new(simbols);

        TaskObjectSpawner.Spawn(pcPrefab);

        for (int i = 0; i < codeLength; i++)
        {
            TaskObjectSpawner.Spawn(papersPrefab);
        }

        Papers.OnTake += AddSymbol;
        PC.CodeLength = codeLength;
    }

    private void OnDisable()
    {
        Papers.OnTake -= AddSymbol;
    }

    private void Update()
    {
        char[] symbols = new char[codeLength];

        for (int i = 0; i < curCodeSym; i++)
        {
            symbols[i] = code[i];
        }

        for (int i = curCodeSym; i < codeLength; i++)
        {
            symbols[i] = '*';
        }

        text.text = "Code:\n" + new string(symbols);
    }

    public override bool Check()
    {
        if (PC.HasUsed) Destroy(text.gameObject);
        return PC.HasUsed;
    }
}
