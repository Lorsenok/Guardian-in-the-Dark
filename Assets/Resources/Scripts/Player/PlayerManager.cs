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

    private void Start()
    {
        playerController = Instantiate(playerPrefab, playerSpawnOffset, Quaternion.identity).GetComponent<Controller>();
    }

    public Transform GetPlayerPosition()
    {
        return playerController.transform;
    }

    private void Death()
    {
        Controller.CanMove = false;
    }

    private void Update()
    {
        if (HP <= 0)
        {
            Death();
            return;
        }

        HP -= Time.deltaTime * (hpLossSpeed + AdditionalHPLossSpeed);
    }
}
