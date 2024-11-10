using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskObjectSpawner : MonoBehaviour
{
    public static List<Transform> Spawners { get; set; } = new List<Transform>();

    public static GameObject Spawn(GameObject obj)
    {
        if (Spawners.Count == 0)
        {
            Debug.Log("There is no place to spawn task object!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return null;
        }

        int rand = Random.Range(0, Spawners.Count - 1);

        GameObject final = Instantiate(obj, Spawners[rand].transform.position, obj.transform.rotation);

        Destroy(Spawners[rand].gameObject);

        return final;
    }

    private void OnEnable()
    {
        Spawners.Add(transform);
    }

    private void OnDisable()
    {
        Spawners.Remove(transform);
    }
}
