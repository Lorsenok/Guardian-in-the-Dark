using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PackageTask : Task
{
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private GameObject packageSpotPrefab;

    private bool hasDelivered = false;

    private void OnDelivered()
    {
        hasDelivered = true;
    }

    private void Awake()
    {
        TaskObjectSpawner.Spawn(packageSpotPrefab);
        TaskObjectSpawner.Spawn(packagePrefab);
    }

    private void Start()
    {
        Package.Instance.OnDelivered += OnDelivered;
    }

    public override bool Check()
    {
        return hasDelivered;
    }
}
