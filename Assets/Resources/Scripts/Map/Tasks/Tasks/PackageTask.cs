using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PackageTask : Task
{
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private GameObject packageSpotPrefab;

    [SerializeField] private PackageTaskFind find;

    private bool hasDelivered = false;

    private void OnDelivered()
    {
        hasDelivered = true;
    }

    private void Start()
    {
        TaskObjectSpawner.Spawn(packageSpotPrefab);
        TaskObjectSpawner.Spawn(packagePrefab);

        Package.Instance.OnDelivered += OnDelivered;
        find.Work();
    }

    public override bool Check()
    {
        return hasDelivered;
    }
}
