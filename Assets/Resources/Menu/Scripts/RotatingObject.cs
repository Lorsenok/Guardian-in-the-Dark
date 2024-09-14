using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    private Vector3 curRotation = Vector3.zero;
    
    private void Update()
    {
        curRotation += direction * Time.deltaTime;

        transform.eulerAngles = curRotation;
    }
}
