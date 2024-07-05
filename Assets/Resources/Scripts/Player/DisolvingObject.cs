using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolvingObject : MonoBehaviour
{
    [SerializeField] private float aliveTime;
    private float startTime;

    private SpriteRenderer spr;

    private void Start()
    {
        startTime = aliveTime;
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, aliveTime / startTime);

        aliveTime -= Time.deltaTime;
    }
}
