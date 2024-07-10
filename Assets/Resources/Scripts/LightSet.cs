using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSet : MonoBehaviour
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private float set;
    [SerializeField] private float speed;

    private void Update()
    {
        light2d.intensity = Mathf.Lerp(light2d.intensity, set, speed * Time.deltaTime);
    }
}
