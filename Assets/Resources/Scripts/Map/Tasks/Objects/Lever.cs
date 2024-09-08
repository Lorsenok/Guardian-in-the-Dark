using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : UsableObject
{
    private bool hasWorked = false;

    public List<Rails> Rails { get; set; } = new();


    [Header("Visuals")]
    [SerializeField] private Transform[] model;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotate;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textColoringSpeed;
     
    private Color startTextColor;

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
        if (canBeTaked && !hasWorked)
        {
            text.color = Color.Lerp(text.color, startTextColor, Time.deltaTime * textColoringSpeed);

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (Rails r in Rails)
                {
                    r.IsWork = true;
                }
                hasWorked = true;
            }
        }
        else
            text.color = Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textColoringSpeed);

        if (hasWorked)
        {
            foreach(Transform t in model)
            {
                t.transform.rotation = Quaternion.Lerp(t.transform.rotation, Quaternion.Euler(rotate), Time.deltaTime * speed);
            }
        }
    }
}
