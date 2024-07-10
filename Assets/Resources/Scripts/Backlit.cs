using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Backlit : MonoBehaviour
{
    public Color color;

    [SerializeField] private float speed;
    [SerializeField] private float lightTimeSet;
    private float lightTime;

    public Light2D Lighting;

    public void Light()
    {
        lightTime = lightTimeSet;
    }

    private void Start()
    {
        Lighting.intensity = 0;
    }

    private float curLight;
    private void Update()
    {
        if (lightTime > 0) lightTime -= Time.deltaTime;

        curLight = 1 / lightTimeSet * lightTime;

        Lighting.intensity = Mathf.Lerp(Lighting.intensity, curLight, Time.deltaTime * speed);
    }
}
