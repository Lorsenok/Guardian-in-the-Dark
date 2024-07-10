using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 playerSpawnOffset;

    [SerializeField] private float hpLossSpeed;

    public float HP { get; set; } = 1000;

    public float AdditionalHPLossSpeed { get; set; } = 0;

    private Controller playerController;

    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
        playerController = Instantiate(playerPrefab, playerSpawnOffset, Quaternion.identity).GetComponent<Controller>();
    }

    private void Start()
    {
        foreach (GameObject em in Config.EnemyManagers)
        {
            Instantiate(em, Vector3.zero, Quaternion.identity);
        }
    }

    public Transform GetPlayerPosition()
    {
        if (playerController == null) return null;
        return playerController.transform;
    }

    private void Death()
    {
        Controller.CanMove = false;
    }

    private void Update()
    {
        if (HP <= -0.01f)
        {
            Death();
            return;
        }

        HP -= Time.deltaTime * (hpLossSpeed + AdditionalHPLossSpeed);
    }
}
