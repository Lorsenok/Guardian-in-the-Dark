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
    [SerializeField] private TextMeshProUGUI[] text;
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
        if (!curRails.Contains(rails))
        {
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
        foreach (TextMeshProUGUI text in text)
        {
            textStartColor = text.color;
            text.color = new Color(0, 0, 0, 0);
        }
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

        bool canMove = true;
        bool canMoveBackwards = true;

        foreach (Rails r in curRails)
        {
            if (!r.IsWork)
            {
                canMove = false;
            }

            if (r.IsTheFirst)
            {
                canMoveBackwards = false;
            }
        }

        if (canMove) transform.position += (Vector3)direction * curSpeed * (Input.GetKey(KeyCode.F) & canMoveBackwards ? -1 : 1);
        else if (canMoveBackwards && Input.GetKey(KeyCode.F)) transform.position += (Vector3)direction * curSpeed * -1;

        if (canBeTaked)
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) & canMoveBackwards)
            {
                foreach (TextMeshProUGUI text in text)
                    text.color = Color.Lerp(text.color, new Color(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);
                takeTime += Time.deltaTime * accelerationSpeed;
                curSpeed = Mathf.Lerp(curSpeed, speed, Time.deltaTime * ProjMath.EaseOutQuint(takeTime) * accelerationSpeed);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, curRotation), Time.deltaTime * rotationSpeed);
            }
            else
            {
                foreach (TextMeshProUGUI text in text)
                    text.color = Color.Lerp(text.color, textStartColor, Time.deltaTime * textColoringSpeed);
                takeTime = 0;
                curSpeed = Mathf.Lerp(curSpeed, 0, Time.deltaTime * deccelerationSpeed);
            }
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, Time.deltaTime * deccelerationSpeed);
            foreach (TextMeshProUGUI text in text)
                text.color = Color.Lerp(text.color, new Color(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);
            takeTime = 0;
        }

        Rails railsToSpawn = null;

        if (curRails.Count == 0)
        {
            float curDist = 1000f;

            foreach (Rails r in curRails)
            {
                float dist = Vector2.Distance(r.transform.position, transform.position);
                if (dist < curDist)
                {
                    railsToSpawn = r;
                    curDist = dist;
                }
            }
        }

        if (railsToSpawn != null)
        {
            transform.position = railsToSpawn.transform.position;
        }
    }
}
