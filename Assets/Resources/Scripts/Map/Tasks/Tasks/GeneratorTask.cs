using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTask : Task
{
    private int count = 0;

    private void OnGeneratorUsed()
    {
        count--;
    }

    private void Start()
    {
        count = FindObjectsOfType<Generator>().Length; // I need to replace Find somehow lol
        Generator.OnUse += OnGeneratorUsed;
    }

    public override bool Check()
    {
        return count <= 0;
    }
}
