using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolvingObject : MonoBehaviour
{
    [SerializeField] private bool opacity = true;
    [SerializeField] private float aliveTime;
    private float startTime;

    private SpriteRenderer spr;

    private void Start()
    {
        startTime = aliveTime;
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        if (opacity) spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, aliveTime / startTime);
        else spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1);

        aliveTime -= Time.deltaTime;
    }
}
