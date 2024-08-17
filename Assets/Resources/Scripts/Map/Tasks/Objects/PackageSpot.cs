using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpot : MonoBehaviour
{
    public static Transform InstancePosition { get; private set; }

    private void Awake()
    {
        InstancePosition = transform;
    }
}
