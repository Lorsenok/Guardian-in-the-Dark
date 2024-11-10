using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : UsableObject
{
    public List<Rails> Rails { get; set; } = new();

    [Header("Lever")]
    [SerializeField] private float workTime;

    private float curWorkTime = 0f;

    [Header("Visuals")]
    [SerializeField] private Transform[] model;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotate;
    [SerializeField] private Vector3 startRotate;
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textColoringSpeed;
     
    private Color startTextColor;

    private bool isWorking = false;

    private void Start()
    {
        foreach (Rails r in Rails)
        {
            r.IsWork = false;
        }
        startTextColor = text.color;
    }

    private void Update()
    {
        if (isWorking)
        {
            curWorkTime -= Time.deltaTime;

            if (curWorkTime < 0f)
            {
                isWorking = false;
            }
        }

        if (canBeTaked && !isWorking)
        {
            text.color = Color.Lerp(text.color, startTextColor, Time.deltaTime * textColoringSpeed);

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (Rails r in Rails)
                {
                    r.IsWork = true;
                }
                isWorking = true;

                curWorkTime = workTime;
            }
        }
        else
            text.color = Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);

        foreach (Transform t in model)
        {
            t.transform.rotation = Quaternion.Lerp(t.transform.rotation, isWorking ? Quaternion.Euler(rotate) : Quaternion.Euler(startRotate), Time.deltaTime * speed);
        }
    }
}
