using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : UsableObject
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float textColorChangeSpeed;

    private Color startTextColor;

    private bool hasTaken = false;

    [SerializeField] private Transform firstHalf;
    private float firstStartY;
    [SerializeField] private Transform secondHalf;
    private float secondStartY;

    [SerializeField] private float openDistance;

    private void Start()
    {
        startTextColor = text.color;

        firstStartY = firstHalf.localPosition.y;
        secondStartY = secondHalf.localPosition.y;
    }

    private float time;

    private void Update()
    {
        text.color = canBeTaked && !hasTaken ? Color.Lerp(text.color, startTextColor, Time.deltaTime * textColorChangeSpeed) : Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textColorChangeSpeed);

        if (canBeTaked && Input.GetKeyDown(KeyCode.E)) 
        {
            hasTaken = true;
        }

        if (!hasTaken) return;


        time += Time.deltaTime;
        if (time > 1) time = 1;

        firstHalf.localPosition = new Vector3(firstHalf.localPosition.x, firstStartY + ProjMath.EaseInBounce(time), firstHalf.position.z);
        secondHalf.localPosition = new Vector3(secondHalf.localPosition.x, secondStartY - ProjMath.EaseInBounce(time), secondHalf.position.z);
    }
}
