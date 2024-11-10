using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Generator : UsableObject
{
    private bool hasUsed = false;

    public static Action OnUse;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform line;
    [SerializeField] private SpriteRenderer lineSpr;
    [SerializeField] private Transform model;

    [SerializeField] private float holdTime;

    [Header("Visuals")]
    
    [SerializeField] private float lineColoringSpeed;
    [SerializeField] private float textColoringSpeed;
    
    [SerializeField] private float cameraShakePower;
    [SerializeField] private float maxCameraShakeDistance;

    [SerializeField] private float objectShakePower;
    [SerializeField] private float objectShakeSpeed;
    [SerializeField] private float objectShakeDelay;

    private Vector3 startPos;
    private Vector3 curShakePos;

    private Color startTextColor;

    private float startLineWidth;
    private Color startLineColor;

    private float curHoldTime = 0;
    private float curObjectShakeDelay = 0;

    private void Start()
    {
        startLineColor = lineSpr.color;
        startLineWidth = line.transform.localScale.x;
        startTextColor = text.color;
        startPos = model.transform.position;

        line.transform.localScale = new Vector3(0, line.localScale.y, 1);
        lineSpr.color = new(0, 0, 0, 0);
    }

    private void Update()
    {
        if (hasUsed)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < maxCameraShakeDistance)
            {
                CameraShakeManager.Instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), cameraShakePower / dist);
            }

            lineSpr.color = Color.Lerp(lineSpr.color, new(0, 0, 0, 0), Time.deltaTime * lineColoringSpeed);
            line.localScale = Vector3.Lerp(line.localScale, new(0, 0, 0), Time.deltaTime);

            curObjectShakeDelay -= Time.deltaTime;
            if (curObjectShakeDelay <= 0)
            {
                float rand1 = UnityEngine.Random.Range(-objectShakePower, objectShakePower);
                float rand2 = UnityEngine.Random.Range(-objectShakePower, objectShakePower);
                curShakePos = startPos + new Vector3(rand1, rand2, 0);
                curObjectShakeDelay = objectShakeDelay;
            }

            model.position = Vector3.Lerp(model.position, curShakePos, Time.deltaTime * objectShakeSpeed);

            return;
        }

        if (curHoldTime > holdTime)
        {
            hasUsed = true;
            OnUse?.Invoke();
        }
        else if (canBeTaked)
        {
            if (Input.GetKey(KeyCode.E))
            {
                curHoldTime += Time.deltaTime;
                text.color = Color.Lerp(text.color, new(0, 0, 0, 0), textColoringSpeed * Time.deltaTime);

                line.localScale = Vector3.Lerp(line.localScale, new(startLineWidth / holdTime * curHoldTime, line.localScale.y, 1), Time.deltaTime);

                lineSpr.color = Color.Lerp(lineSpr.color, startLineColor * new Color(1 - 1 / holdTime * curHoldTime, 1, 1 - 1 / holdTime * curHoldTime, 1), Time.deltaTime * lineColoringSpeed);
            }
            else
            {
                text.color = Color.Lerp(text.color, startTextColor, textColoringSpeed * Time.deltaTime);

                line.localScale = Vector3.Lerp(line.localScale, new(0, line.localScale.y, 1), Time.deltaTime);
                lineSpr.color = Color.Lerp(lineSpr.color, new(0, 0, 0, 0), Time.deltaTime * lineColoringSpeed);
            }
        }
        else
        {
            text.color = Color.Lerp(text.color, new(0, 0, 0, 0), textColoringSpeed * Time.deltaTime);
            lineSpr.color = Color.Lerp(lineSpr.color, new(0, 0, 0, 0), Time.deltaTime * lineColoringSpeed);
            line.localScale = Vector3.Lerp(line.localScale, new(0, line.localScale.y, 1), Time.deltaTime);
            curHoldTime = 0;
        }
    }
}
