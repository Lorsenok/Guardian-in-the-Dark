using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WingedEnemyWing : MonoBehaviour
{
    public static List<WingedEnemyWing> WingedEnemyWings = new List<WingedEnemyWing>();

    [SerializeField] private bool destroyWithOwner;
    [SerializeField] private bool followZPosition;

    private float startZPosition;

    private Enemy enemy;

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (destroyWithOwner) Enemy.OnEnemyDestroyed -= OnDie;
    }

    private void OnEnable()
    {
        WingedEnemyWings.Add(this);
    }

    private void OnDisable()
    {
        WingedEnemyWings.Remove(this);
    }

    private void Start()
    {
        enemy = EnemyManager.LastEnemy;

        if (destroyWithOwner) Enemy.OnEnemyDestroyed += OnDie;

        startZPosition = transform.position.z;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition() && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            PlayerManager.Instance.IsDeadByEnemy = true;
            CameraShakeManager.Instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), enemy.ShakePower);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition() && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            PlayerManager.Instance.IsDeadByEnemy = true;
            CameraShakeManager.Instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), enemy.ShakePower);
        }
    }

    private void Update()
    {
        if (!followZPosition) return;

        transform.position = new Vector3(transform.position.x, transform.position.y, EnemyManager.LastEnemy.transform.position.z * 2 - startZPosition);
    }
}
