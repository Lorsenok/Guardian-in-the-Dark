using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Trolley : UsableObject
{
    public bool IsWork { get; set; } = true;

    private Vector2 direction;

    [SerializeField] private float speed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float deccelerationSpeed;

    private float curSpeed;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float timeBeforeDestroy;


    [Header("Visuals")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform canvas;
    [SerializeField] private float textColoringSpeed;
    [SerializeField] private Transform model;
    [SerializeField] private float dissapearSpeed;

    private Color textStartColor;

    private List<Rails> curRails = new();

    private float takeTime = 0f;

    private float curRotation = 0f;

    private Quaternion startTextRotation;

    public void OnCollide(Rails rails)
    {
        if (!curRails.Contains(rails) && rails.IsWork)
        {
            curRails.Add(rails);
            direction += rails.Direction;
        }

        if (rails.Rotate)
        {
            curRotation = rails.Rotation;
        }
    }

    public void OnCollideExit(Rails rails)
    {
        if (curRails.Contains(rails))
        {
            curRails.Remove(rails);
            direction -= rails.Direction;
        }
    }

    private void Start()
    {
        textStartColor = text.color;
        text.color = new Color(0, 0, 0, 0);
        startTextRotation = canvas.transform.localRotation;
    }

    private void Update()
    {
        canvas.transform.rotation = startTextRotation;

        if (!IsWork)
        {
            timeBeforeDestroy -= Time.deltaTime;
            model.localScale = Vector3.Lerp(model.localScale, Vector3.zero, dissapearSpeed * Time.deltaTime);

            if (timeBeforeDestroy <= 0)
            {
                Destroy(gameObject);
            }

            return;
        }

        transform.position += (Vector3)direction * curSpeed;

        if (canBeTaked)
        {
            if (Input.GetKey(KeyCode.E))
            {
                text.color = Color.Lerp(text.color, new Color(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);
                takeTime += Time.deltaTime * accelerationSpeed;
                curSpeed = Mathf.Lerp(curSpeed, speed, Time.deltaTime * ProjMath.EaseOutQuint(takeTime) * accelerationSpeed);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, curRotation), Time.deltaTime * rotationSpeed);
            }
            else
            {
                text.color = Color.Lerp(text.color, textStartColor, Time.deltaTime * textColoringSpeed);
                takeTime = 0;
                curSpeed = Mathf.Lerp(curSpeed, 0, Time.deltaTime * deccelerationSpeed);
            }
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, Time.deltaTime * deccelerationSpeed);
            text.color = Color.Lerp(text.color, new Color(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);
            takeTime = 0;
        }
    }
}
