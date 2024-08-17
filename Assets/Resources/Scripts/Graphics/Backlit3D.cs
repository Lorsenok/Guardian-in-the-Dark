using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Backlit3D : MonoBehaviour
{
    public Color color;

    [SerializeField] private Color colorSet;

    [SerializeField] private float speed;
    [SerializeField] private float appearSpeed;
    [SerializeField] private float dissapearSpeed;
    [SerializeField] private float lightTimeSet;
    [SerializeField] private float intensity;
    [SerializeField] private float divisions;
    [SerializeField] private bool coloringByLight;
    private float lightTime;

    private float startSmoothness;

    public Light Lighting;

    [SerializeField] private Material material;

    public void Light()
    {
        lightTime += lightTimeSet / divisions;
        lightTime = Mathf.Clamp(lightTime, 0f, lightTimeSet);
    }

    private void Start()
    {
        Lighting.intensity = 0;
        material.color = Color.black;
        startSmoothness = material.GetFloat("_Smoothness");
        material.SetFloat("_Smoothness", 0);
    }

    private float curLight;
    private void Update()
    {
        if (!coloringByLight)
        {
            material.color = Color.Lerp(material.color, colorSet, Time.deltaTime * appearSpeed);
            material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness, appearSpeed));
        }
        else
        {
            material.color = Color.Lerp(material.color, colorSet / lightTimeSet * lightTime, Time.deltaTime * appearSpeed);
            material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness / lightTimeSet * lightTime, Time.deltaTime * appearSpeed));
        }

        if (lightTime > 0) lightTime -= Time.deltaTime * dissapearSpeed;

        curLight = 1 / lightTimeSet * lightTime;
        curLight *= intensity;

        Lighting.intensity = Mathf.Lerp(Lighting.intensity, curLight, Time.deltaTime * speed);
    }
}
