using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagersManager : MonoBehaviour
{
    [SerializeField] private float timeBeforeSpawn = 1f;
    private bool hasWorked = false;

    private List<EnemyManager> enemyManagers = new();

    private void OnEnemyDestroyed()
    {
        List<AffiliationPoint> points = new List<AffiliationPoint>();

        foreach (EnemyManager em in enemyManagers)
        {
            for (int i = 0; i < em.SpawnChance; i++)
            {
                points.Add(new AffiliationPoint(em));
            }
        }

        EnemyManager enemy = points[Random.Range(0, points.Count)].Affiliation as EnemyManager;
        enemy.enabled = true;
        Debug.Log(enemy.ToString() + " is spawning now");

        foreach (EnemyManager em in enemyManagers)
        {
            if (em != enemy) em.enabled = false;
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= OnEnemyDestroyed;
    }

    private void Update()
    {
        timeBeforeSpawn -= Time.deltaTime;
        if (timeBeforeSpawn > 0f || hasWorked) return;

        hasWorked = true;

        if (Config.EnemyManagers != null)
        {
            if (Config.EnemyManagers.Length == 0)
            {
                Config.EnemyManagers = null;
            }
        }

        if (Config.EnemyManagers != null)
        {
            foreach (GameObject em in Config.EnemyManagers)
            {
                enemyManagers.Add(Instantiate(em, Vector3.zero, Quaternion.identity).GetComponent<EnemyManager>());
            }

            foreach (EnemyManager em in enemyManagers)
            {
                Debug.Log(em.ToString());
                em.enabled = false;
            }

            HuntTask.IsEnemiesExist = true;
        }
        else
        {
            HuntTask.IsEnemiesExist = false;
            Destroy(gameObject);
            return;
        }

        OnEnemyDestroyed();
    }
}
