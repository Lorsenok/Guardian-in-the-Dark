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

    public void Work()
    {
        Package.Instance.OnTake += OnTaked;
    }

    public override bool Check()
    {
        return hasTaked;
    }
}
