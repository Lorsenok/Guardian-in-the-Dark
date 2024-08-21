using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public static CameraSetup Instance { get; private set; }

    [SerializeField] private float speed;

    public Camera Camera2D { get; private set; }
    public Camera Camera3D { get; private set; }

    [SerializeField] private Camera camera2D;
    [SerializeField] private Camera camera3D;

    private Transform player;

    private void Awake()
    {
        Instance = this;

        Camera2D = camera2D;
        Camera3D = camera3D;
    }

    private void FixedUpdate() // I haven't use cinemachine follow bc its follow player is not convenient
    {
        if (player == null & PlayerManager.Instance.GetPlayerPosition() == null) return;
        else if (player == null)
        {
            player = PlayerManager.Instance.GetPlayerPosition();
            transform.position = player.position;
        }

        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
