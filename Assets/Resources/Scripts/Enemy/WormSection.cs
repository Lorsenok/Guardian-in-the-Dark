using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSection : MonoBehaviour
{
    public WormEnemy Enemy { get; set; }
    [SerializeField] private Transform model;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition() && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            CameraShakeManager.instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), Enemy.ShakePower);
        }
    }
}
