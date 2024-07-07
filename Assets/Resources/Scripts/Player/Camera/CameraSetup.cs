using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float speed;

    private Transform player;

    private void FixedUpdate() // I haven't use cinemachine follow bc its follow player is not convenient
    {
        if (player == null & playerManager.GetPlayerPosition() == null) return;
        else if (player == null) player = playerManager.GetPlayerPosition();

        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
