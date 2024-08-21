using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Backlit3D : MonoBehaviour
{
    public Color color;

    private bool hasBeenToched = false;

    [SerializeField] private Color colorSet;

    [SerializeField] private float speed;
    [SerializeField] private float appearSpeed;
    [SerializeField] private float dissapearSpeed;
    [SerializeField] private float lightTimeSet;
    [SerializeField] private float intensity;
    [SerializeField] private float divisions;
    [SerializeField] private bool isColoringByLight = true;
    private float lightTime;

    private static float startSmoothness;
    private static bool hasStartSmoothnessSetted = false;

    public Light Lighting;

    [SerializeField] private Material material;
    [SerializeField] private Material[] additionalMaterials;

    public List<Material> Materials { get; private set; } = new();

    public void Light()
    {
        lightTime += lightTimeSet / divisions;
        lightTime = Mathf.Clamp(lightTime, 0f, lightTimeSet);
        hasBeenToched = true;
    }

    private void Awake()
    {
        Materials.Add(material);
        foreach (Material m in additionalMaterials)
        {
            Materials.Add(m);
        }
    }

    private void OnDestroy()
    {
        material.SetFloat("_Smoothness", startSmoothness);
    }

    private void Start()
    {
        Lighting.intensity = 0;
        material.color = Color.black;

        if (startSmoothness == 0f && !hasStartSmoothnessSetted)
        {
            hasStartSmoothnessSetted = true;
            startSmoothness = material.GetFloat("_Smoothness");
            material.SetFloat("_Smoothness", 0);
        }

        if (additionalMaterials != null)
        {
            foreach (Material material in additionalMaterials)
            {
                material.color = Color.black;
                material.SetFloat("_Smoothness", 0);
            }
        }
    }

    private float curLight;
    private void Update()
    {
        if (!hasBeenToched) return;

        if (!isColoringByLight)
        {
            material.color = Color.Lerp(material.color, colorSet, Time.deltaTime * appearSpeed);
            material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness, Time.deltaTime * appearSpeed));

            if (additionalMaterials != null)
            {
                foreach (Material material in additionalMaterials)
                {
                    material.color = Color.Lerp(material.color, colorSet, Time.deltaTime * appearSpeed);
                    material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness, Time.deltaTime * appearSpeed));
                }
            }
        }
        else
        {
            material.color = Color.Lerp(material.color, colorSet / lightTimeSet * lightTime, Time.deltaTime * appearSpeed);
            material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness / lightTimeSet * lightTime, Time.deltaTime * appearSpeed));

            if (additionalMaterials != null)
            {
                foreach (Material material in additionalMaterials)
                {
                    material.color = Color.Lerp(material.color, colorSet / lightTimeSet * lightTime, Time.deltaTime * appearSpeed);
                    material.SetFloat("_Smoothness", Mathf.Lerp(material.GetFloat("_Smoothness"), startSmoothness / lightTimeSet * lightTime, Time.deltaTime * appearSpeed));
                }
            }
        }

        if (lightTime > 0) lightTime -= Time.deltaTime * dissapearSpeed;

        curLight = 1 / lightTimeSet * lightTime;
        curLight *= intensity;

        Lighting.intensity = Mathf.Lerp(Lighting.intensity, curLight, Time.deltaTime * speed);
    }
}
