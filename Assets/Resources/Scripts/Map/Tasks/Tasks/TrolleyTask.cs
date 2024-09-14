using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyTask : Task
{
    [SerializeField] private GameObject trolleyPrefab;

    private bool end = false;

    private void OnEnd()
    {
        end = true;
    }

    private void OnEnable()
    {
        RailsEnd.OnEnd += OnEnd;

        TaskObjectSpawner.Spawn(trolleyPrefab);
    }

    public override bool Check()
    {
        return end;
    }
}
