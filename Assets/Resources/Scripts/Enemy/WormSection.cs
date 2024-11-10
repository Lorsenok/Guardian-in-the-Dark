using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WormSection : MonoBehaviour
{
    public Enemy Enemy { get; set; }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == PlayerManager.Instance.GetPlayerPosition() && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            CameraShakeManager.Instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), Enemy.ShakePower);
        }
    }
}
