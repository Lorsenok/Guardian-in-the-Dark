using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    private float power;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        power = Config.ShakePower;
    }

    public void Shake(CinemachineImpulseSource source, float _power)
    {
        source.m_DefaultVelocity.x = Random.Range(-1f, 1f);
        source.m_DefaultVelocity.y = Random.Range(-1f, 1f);
        source.GenerateImpulseWithForce(_power * power);
    }
}