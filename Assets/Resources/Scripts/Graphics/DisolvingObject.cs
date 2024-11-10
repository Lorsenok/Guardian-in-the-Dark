using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisolvingObject : MonoBehaviour
{
    public int Index { get; set; } = 0;
    public float CurAlpha { get; set; } = 1f;

    [SerializeField] private bool opacity = true;
    [SerializeField] private float aliveTime;
    private float startTime;

    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Image img;

    [SerializeField] private bool appearByAnotherSctipt = false;
    [SerializeField] private float appearSpeed;

    public bool Appear { get; set; } = false;

    private void Start()
    {
        startTime = aliveTime;

        if (!Appear && appearByAnotherSctipt)
        {
            if (spr != null) spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0f);
            if (img != null) img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        if (opacity)
        {
            if (spr != null)
            {
                spr.color = appearByAnotherSctipt ?
                Color.Lerp(spr.color, new Color(spr.color.r, spr.color.g, spr.color.b, Appear ? aliveTime / startTime : 0f), Time.deltaTime * appearSpeed)
                :
                new Color(spr.color.r, spr.color.g, spr.color.b, aliveTime / startTime);

                CurAlpha = spr.color.a;
            }

            if (img != null)
            {
                img.color = appearByAnotherSctipt ?
                    Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, Appear ? aliveTime / startTime : 0f), Time.deltaTime * appearSpeed)
                    :
                    new Color(img.color.r, img.color.g, img.color.b, aliveTime / startTime);

                CurAlpha = img.color.a;
            }
        }
        
        else
        {
            if (spr != null) spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1f);
            if (img != null) img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        }


        aliveTime -= Time.deltaTime;
    }
}
