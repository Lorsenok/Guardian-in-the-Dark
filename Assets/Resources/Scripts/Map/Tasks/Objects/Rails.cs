using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rails : MonoBehaviour
{
    public bool IsWork { get; set; } = true;

    public Rails LastRails { get; set; }
    public Vector2 Direction { get; set; } = Vector2.zero;

    [SerializeField] private Color colorSet;
    [SerializeField] private float coloringSpeed;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private bool isRotate;

    private Color startColor;

    private Vector2 startDirection;

    private void Start()
    {
        startDirection = Direction;
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

        Direction = IsWork ? startDirection : Vector3.zero;

        if (isRotate)
        {
            spr.flipY = LastRails.Direction.y > 0 & Direction.x <= 0 
                || LastRails.Direction.x < 0 & Direction.y <= 0 
                || LastRails.Direction.x > 0 & Direction.y > 0 
                || LastRails.Direction.y < 0 & Direction.x > 0;
        }
    }
}
