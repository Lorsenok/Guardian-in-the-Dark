using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Package : UsableObject
{
    public static Package Instance { get; private set; }

    public Action OnDelivered { get; set; }
    public Action OnTake { get; set; }

    [SerializeField] private AudioSource sound;
    [SerializeField] private float timeForSoundExpire = 1.0f;

    private float curTimeForSoundExpire = 0f;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Transform destinationPos;
    [SerializeField] private SpriteRenderer destinationSpr;

    [SerializeField] private float destinationAlphaChangeSpeed;
    [SerializeField] private float textColorChangeSpeed;

    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private float minDistanceToSpot;

    private Color startDestinationColor;
    private Color startTextColor;

    private bool hasTaken = false;
     
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
        sound.volume = Config.Sound;

        text.color = canBeTaked && !hasTaken ? Color.Lerp(text.color, startTextColor, Time.deltaTime * textColorChangeSpeed) : Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textColorChangeSpeed);

        if (Vector2.Distance(transform.position, spot.position) < minDistanceToSpot)
        {
            enabled = false;
            destinationSpr.enabled = false;
            sound.Stop();
            return;
        }

        if (canBeTaked | hasTaken && Input.GetKeyDown(KeyCode.E))
        {
            hasTaken = !hasTaken;
            OnTake?.Invoke();
        }

        if (player == null)
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, new Color(0, 0, 0, 0), Time.deltaTime * destinationAlphaChangeSpeed);
            return;
        }

        if (hasTaken)
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, startDestinationColor, Time.deltaTime * destinationAlphaChangeSpeed);

            Vector3 diference = spot.position - transform.position;
            float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
            destinationPos.rotation = Quaternion.Euler(0f, 0f, rotateZ);

            if (Vector2.Distance(player.transform.position, transform.position) > distance)
            {
                Vector3 dir = player.transform.position - transform.position;
                transform.position += dir.normalized * speed * Time.deltaTime;

                if (curTimeForSoundExpire <= 0f)
                {
                    curTimeForSoundExpire = timeForSoundExpire;
                    Debug.Log("+");
                }
            }

        }
        else
        {
            destinationSpr.color = Color.Lerp(destinationSpr.color, new Color(0, 0, 0, 0), Time.deltaTime * destinationAlphaChangeSpeed);
        }

        if (curTimeForSoundExpire > 0f)
        {
            if (!sound.isPlaying) sound.Play();
        }
        else
        {
            sound.Stop();
        }

        if (curTimeForSoundExpire > 0f) curTimeForSoundExpire -= Time.deltaTime;
    }
}
