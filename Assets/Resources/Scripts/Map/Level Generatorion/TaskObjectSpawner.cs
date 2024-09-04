using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObjectSpawner : MonoBehaviour
{
    public static List<Transform> Spawners = new List<Transform>();

    private void OnEnable()
    {
        Spawners.Add(transform);
    }

    private void OnDisable()
    {
        Spawners.Remove(transform);
    }
}
