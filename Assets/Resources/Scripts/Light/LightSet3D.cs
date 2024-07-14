using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSet3D : MonoBehaviour
{
    [SerializeField] private Light light3d;
    [SerializeField] private float set;
    [SerializeField] private float speed;

    private void Update()
    {
        light3d.intensity = Mathf.Lerp(light3d.intensity, set, speed * Time.deltaTime);
    }
}
