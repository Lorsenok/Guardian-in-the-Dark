using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Package : MonoBehaviour
{
    public static Package Instance { get; private set; }

    public Action OnDelivered { get; set; }
    public Action OnTake { get; set; }

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Transform destinationPos;
    [SerializeField] private SpriteRenderer destinationSpr;

    [SerializeField] private float destinationAlphaChangeSpeed;
    [SerializeField] private float textColorChangeSpeed;

    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float minDistanceToSpot;

    [SerializeField] private float playerSpeedSet;

    private Color startDestinationColor;
    private Color startTextColor;

    public bool HasTaken { get; private set; } = false;

    private bool canBeTaked = false;

    private Controller player;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Controller>(out player))
        {
            canBeTaked = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Controller>(out player))
        {
            canBeTaked = false;
        }
    }
     
    private void OnDisable()
    {
        OnDelivered?.Invoke();
    }

    private void Awake()
    {
        Instance = this;
    }

    private Transform spot;

    private void Start()
    {
        spot = PackageSpot.InstancePosition;
        startDestinationColor = destinationSpr.color;
        startTextColor = text.color;
    }

    private void Update()
    {
        text.color = canBeTaked && !HasTaken ? Color.Lerp(text.color, startTextColor, Time.deltaTime * textColorChangeSpeed) : Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textColorChangeSpeed);

        if (Vector2.Distance(transform.position, spot.position) < minDistanceToSpot)
        {
            enabled = false;
            destinationSpr.enabled = false;
            return;
        }

        if (canBeTaked | HasTaken && Input.GetKeyDown(KeyCode.E))
        {
            HasTaken = !HasTaken;
            if (HasTaken) OnTake?.Invoke();
        }

        if (player == null)
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, new Color(0, 0, 0, 0), Time.deltaTime * destinationAlphaChangeSpeed);
            return;
        }

        if (HasTaken)
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, startDestinationColor, Time.deltaTime * destinationAlphaChangeSpeed);

            Vector3 diference = spot.position - transform.position;
            float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
            destinationPos.rotation = Quaternion.Euler(0f, 0f, rotateZ);

            player.CurrectSpeed = playerSpeedSet;

            if (Vector2.Distance(player.transform.position, transform.position) > distance)
            {
                Vector3 dir = player.transform.position - transform.position;
                transform.position += dir.normalized * speed * Time.deltaTime;
            }
        }
        else
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, new Color(0, 0, 0, 0), Time.deltaTime * destinationAlphaChangeSpeed);
        }
    }
}
