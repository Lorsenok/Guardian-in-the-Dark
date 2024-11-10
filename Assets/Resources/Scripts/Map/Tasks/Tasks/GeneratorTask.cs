using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTask : Task
{
    [SerializeField] private GameObject generatorPrefab;
    [SerializeField] private int generatorsQuantity;
    private List<GameObject> generators = new List<GameObject>();

    private int count = 0;

    private void OnGeneratorUsed()
    {
        count--;
    }

    private void OnEnable()
    {
        for (int i = 0; i < generatorsQuantity; i++)
        {
            generators.Add(TaskObjectSpawner.Spawn(generatorPrefab));
        }

        count = generators.Count;

        Generator.OnUse += OnGeneratorUsed;
    }

    public override bool Check()
    {
        return count <= 0;
    }
}
