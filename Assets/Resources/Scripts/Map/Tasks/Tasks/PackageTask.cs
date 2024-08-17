using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PackageTask : Task
{
    private bool hasDelivered = false;

    private void OnDelivered()
    {
        hasDelivered = true;
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
