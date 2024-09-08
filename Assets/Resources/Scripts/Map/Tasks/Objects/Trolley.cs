using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Trolley : UsableObject
{
    [SerializeField] private GameObject railsSpawnerPrefab;

    private bool atOnceSpawnOnTemplate = false;

    public bool IsWork { get; set; } = true;

    private Vector2 direction;

    [SerializeField] private float speed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float deccelerationSpeed;

    private float curSpeed;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float timeBeforeDestroy;


    [Header("Visuals")]
    [SerializeField] private float additionalRotation;
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
            if (!rails.IsWork)
            {
                Debug.Log("+");
                curRails.Clear();
                curSpeed = 0f;

                return;
            }

            curRails.Add(rails);
            direction += rails.Direction;
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

        Instantiate(railsSpawnerPrefab, transform.position, railsSpawnerPrefab.transform.rotation);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (!atOnceSpawnOnTemplate && collision.gameObject.TryGetComponent(out LevelTemplate template))
        {
            atOnceSpawnOnTemplate = true;
            transform.rotation = Quaternion.Euler(0, 0, ProjMath.RotateTowardsPosition(transform.position, transform.position + new Vector3(template.Direction.x, template.Direction.y, 0) * -1) + additionalRotation);
        }
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

        curRotation = ProjMath.RotateTowardsPosition(transform.position, transform.position + (Vector3)direction) + additionalRotation;

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
