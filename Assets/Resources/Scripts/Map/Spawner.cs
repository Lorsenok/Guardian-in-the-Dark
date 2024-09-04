using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject debugObject;

    private void OnEnable()
    {
        if (debugObject != null)
        {
            Instantiate(debugObject);
            Destroy(gameObject);
            return;
        }

        if (objects == null)
        {
            Debug.LogWarning("There is no objects!");
            Destroy(gameObject);
            return;
        }

        Instantiate(objects[Random.Range(0, objects.Count - 1)]);

        Destroy(gameObject);
    }
}
