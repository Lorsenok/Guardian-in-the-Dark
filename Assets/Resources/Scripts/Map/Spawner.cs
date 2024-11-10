using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject delaySpawnObject;
    [SerializeField] private float delaySpawnTime = 1f;

    [SerializeField] private GameObject debugObject;

    private void OnEnable()
    {
        if (debugObject != null)
        {
            Instantiate(debugObject);
            return;
        }

        if (objects == null)
        {
            Debug.Log("There is no objects!");
            return;
        }

        int rand = Random.Range(0, objects.Count);
        Instantiate(objects[rand]);
    }

    private void Update()
    {
        delaySpawnTime -= Time.deltaTime;

        if (delaySpawnTime <= 0)
        {
            Instantiate(delaySpawnObject);
            Destroy(gameObject);
        }
    }
}
