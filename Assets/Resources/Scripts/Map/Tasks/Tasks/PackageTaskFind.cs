using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTaskFind : Task
{
    private bool hasTaked = false;

    private void OnTaked()
    {
        hasTaked = true;
    }

    private void Start()
    {
        Package.Instance.OnTake += OnTaked;
    }

    private void OnDisable()
    {
        Package.Instance.OnTake -= OnTaked;
    }

    public override bool Check()
    {
        return hasTaked;
    }
}
