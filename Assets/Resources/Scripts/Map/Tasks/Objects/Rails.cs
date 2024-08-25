using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rails : MonoBehaviour
{
    public bool IsWork { get; set; } = true;

    public Vector2 Direction { get; private set; }
    public bool Rotate { get; private set; }
    public float Rotation { get; private set; }

    [SerializeField] private Vector2 direction;
    [SerializeField] private bool rotate = false;
    [SerializeField] private float rotation = 0f;

    [SerializeField] private Color colorSet;
    [SerializeField] private float coloringSpeed;
    [SerializeField] private SpriteRenderer spr;

    private Color startColor;

    private void Start()
    {
        Direction = direction;
        Rotation = rotation;
        Rotate = rotate;

        startColor = spr.color;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out Trolley trolley);
        if (trolley != null)
        {
            trolley.OnCollideExit(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.TryGetComponent(out Trolley trolley);
        if (trolley != null)
        {
            trolley.OnCollide(this);
        }
    }

    private void LateUpdate()
    {
        spr.color = Color.Lerp(spr.color, IsWork ? startColor : colorSet, Time.deltaTime * coloringSpeed);

        Direction = IsWork ? direction : Vector3.zero;
    }
}
