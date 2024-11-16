using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial3: MonoBehaviour
{
    private float alpha = 1f;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI[] spr;
    [SerializeField] private Enemy enemy;

    private Color startColor;

    private bool isWorking = false;
    private bool canWork = false;

    private Vector3 playerPosSet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition())
        {
            canWork = true;
            playerPosSet = PlayerManager.Instance.GetPlayerPosition().position;
        }
    }

    private void Start()
    {
        Enemy.OnEnemyDied += OnEnemyDestroyed;
        startColor = spr[0].color;
    }

    private void Update()
    {
        if (isEnd)
        {
            alpha = Mathf.Lerp(alpha, 0f, Time.deltaTime * speed);
            return;
        }

        alpha = Mathf.Lerp(alpha, (canWork & !isWorking) ? 1f : 0f, Time.deltaTime * speed);
        foreach (TextMeshProUGUI text in spr) text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        enemy.enabled = isWorking;

        if (canWork & !isWorking)
        {
            PlayerManager.Instance.GetPlayerPosition().position = playerPosSet;
        }

        Weapon.Instance.AllowSwitching = canWork;

        if (Input.GetKeyDown(KeyCode.Q) && Weapon.Instance.AllowSwitching)
        {
            isWorking = true;
        }

        if (alpha < 0f) alpha = 0f;
    }

    private bool isEnd = false;
    private void OnEnemyDestroyed()
    {
        isEnd = true;
    }
}
