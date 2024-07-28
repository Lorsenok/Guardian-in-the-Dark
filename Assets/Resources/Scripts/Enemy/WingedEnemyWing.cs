using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedEnemyWing : MonoBehaviour
{
    [SerializeField] private Enemy Enemy;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition() && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            CameraShakeManager.instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), Enemy.ShakePower);
        }
    }
}
