using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctupusPartPoint : MonoBehaviour
{
    private Vector3 diffPosition; 

    private void Start()
    {
        if (EnemyManager.LastEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        diffPosition = transform.position - EnemyManager.LastEnemy.transform.position;
    }

    private void Update()
    {
        if (EnemyManager.LastEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = EnemyManager.LastEnemy.transform.position + diffPosition;
    }
}
