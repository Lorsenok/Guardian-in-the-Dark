using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [SerializeField] private GameObject fakeEnemy;
    [SerializeField] private GameObject enemy;

    [SerializeField] private float additionalAngle;
    [SerializeField] private float spawnDistance;

    [SerializeField] private int enemyQuantity;

    [SerializeField] private int borderLayerMask;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spr; 
    [SerializeField] private float appearSpeed;

    public bool IsEnemyKilled { get; set; } = false;

    private Vector3 playerPosSet;

    private List<GameObject> borders = new List<GameObject>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == borderLayerMask && !borders.Contains(collision.gameObject))
        {
            borders.Add(collision.gameObject);
        }
    }

    private void OnDestroyEnemy()
    {
        IsEnemyKilled = true;
    }

    private void Awake()
    {
        Enemy.OnEnemyDestroyed += OnDestroyEnemy;
    }

    private void Start()
    {
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0);

        playerPosSet = PlayerManager.Instance.GetPlayerPosition().position;
        transform.position = playerPosSet;

        int rand = Random.Range(0, enemyQuantity);

        for (int i = 0; i < enemyQuantity; i++)
        {
            Transform obj = Instantiate(i == rand ? enemy : fakeEnemy, transform.position, Quaternion.identity).transform;
            ProjMath.MoveTowardsAngle(obj, (360 / enemyQuantity * i) * -1 + additionalAngle, spawnDistance);
        }
    }

    private float time = 0f;

    private void OnDestroy()
    {
        foreach (GameObject obj in borders)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    private void Update()
    {
        if (borders.Count > 0)
        {
            foreach (GameObject obj in borders)
            {
                obj.SetActive(false);
            }
        }

        PlayerManager.Instance.GetPlayerPosition().position = playerPosSet;

        time += Time.deltaTime * (IsEnemyKilled ? -1f : 1f);
        if (time > 1f) time = 1f;
        else if (time < 0f && IsEnemyKilled) Destroy(gameObject); 

        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, Mathf.Sin(time) * 1.2f);
    }
}
