using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarBomb : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg;

    [SerializeField] private Transform directon;

    [SerializeField] private float push;
    [SerializeField] private float beforeExplodeTime;
    [SerializeField] private float explodeTime;

    [SerializeField] private SpriteRenderer beforeExplodeSpr;
    [SerializeField] private SpriteRenderer onExplodeSpr;

    [SerializeField] private Vector3 sizeSet;
    [SerializeField] private float sizeSetSpeed;

    [SerializeField] private float sineTimeSpeed;

    [Header("points")]
    [SerializeField] private int points;
    [SerializeField] private Transform pointsDirSet;
    [SerializeField] private Transform pointsDir;

    [SerializeField] private bool spawnRays;

    private float startExplodeTime;

    private bool hasExploded = false;

    private void Start()
    {
        rg.AddForce((directon.position - transform.position) * push, ForceMode2D.Impulse);

        onExplodeSpr.color = new Color(onExplodeSpr.color.r, onExplodeSpr.color.g, onExplodeSpr.color.b, 0);

        startExplodeTime = explodeTime;
    }

    private void Update()
    {
        beforeExplodeTime -= Time.deltaTime;

        beforeExplodeSpr.color = new Color(beforeExplodeSpr.color.r, beforeExplodeSpr.color.g, beforeExplodeSpr.color.b, ProjMath.SinTime(m: sineTimeSpeed));

        if (beforeExplodeTime > 0) return;

        if (!hasExploded)
        {
            hasExploded = true;

            for (int i = 0; i < points; i++)
            {
                pointsDirSet.rotation = Quaternion.Euler(0, 0, 360f / points * i);
                Weapon.Instance.Lidar.SpawnRay(transform.position, pointsDir.position, spawnRay: spawnRays);
            }
        }

        rg.velocity = Vector3.zero;
        beforeExplodeSpr.gameObject.SetActive(false);
        explodeTime -= Time.deltaTime;

        onExplodeSpr.color = new Color(onExplodeSpr.color.r, onExplodeSpr.color.g, onExplodeSpr.color.b, explodeTime / startExplodeTime);
        onExplodeSpr.transform.localScale = Vector3.Lerp(onExplodeSpr.transform.localScale, sizeSet, sizeSetSpeed * Time.deltaTime);

        if (explodeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
