using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 playerSpawnPosition;

    [SerializeField] private float hp;
    [SerializeField] private float hpLossSpeed;

    [SerializeField] private float disolveSpeed;
    [SerializeField] private float lightDisolveSpeed;

    public float HP { get; set; } = 1000;

    public float AdditionalHPLossSpeed { get; set; } = 0;

    private Controller playerController;

    public static PlayerManager Instance { get; set; }

    [SerializeField] private Material playerMaterial;
    [SerializeField] private float dissapearTime;

    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private float particlesZ;

    private bool end = false;

    public bool IsMenuClosed { get; set; } = true;
    private Vector3 startMenuPos;
    [SerializeField] private float menuSpeed;
    [SerializeField] private Vector2 menuCloseDistance;

    private void Awake()
    {
        HP = hp;
        Instance = this;
        playerController = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity).GetComponent<Controller>();
    }

    private void Start()
    {
        foreach (GameObject em in Config.EnemyManagers)
        {
            Instantiate(em, Vector3.zero, Quaternion.identity);
        }

        startMenuPos = menu.transform.localPosition;
    }

    public Transform GetPlayerPosition()
    {
        if (playerController == null) return null;
        return playerController.transform;
    }

    Vector3 startPosition;
    private void Death()
    {
        if (Controller.CanMove)
        {
            Instantiate(bloodParticles, playerController.transform.position + new Vector3(0, 0, particlesZ), Quaternion.identity);
            startPosition = playerController.transform.position;
        }

        Controller.CanMove = false;
        playerMaterial.SetFloat("_Disolve", playerMaterial.GetFloat("_Disolve") + Time.deltaTime * disolveSpeed);
        playerMaterial.SetFloat("_Smoothness", playerMaterial.GetFloat("_Smoothness") - Time.deltaTime * disolveSpeed);

        foreach (Light l in playerController.GetComponentsInChildren<Light>())
        {
            l.intensity = Mathf.Lerp(l.intensity, 0f, Time.deltaTime * lightDisolveSpeed);
        }
        foreach (Light2D l in playerController.GetComponentsInChildren<Light2D>())
        {
            l.intensity = Mathf.Lerp(l.intensity, 0f, Time.deltaTime * lightDisolveSpeed);
        }

        if (playerMaterial.GetFloat("_Disolve") >= dissapearTime)
        {
            foreach (Light l in playerController.GetComponentsInChildren<Light>())
            {
                l.enabled = false;
            }
            foreach (Light2D l in playerController.GetComponentsInChildren<Light2D>())
            {
                l.enabled = false;
            }
            end = true;
        }
    }

    private void OnDisable()
    {
        playerMaterial.SetFloat("_Disolve", 0f);
        playerMaterial.SetFloat("_Smoothness", 0.5f);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) IsMenuClosed = !IsMenuClosed;
        
        menu.transform.localPosition = Vector3.Lerp(menu.transform.localPosition, startMenuPos + (IsMenuClosed ? menuCloseDistance : Vector3.zero), Time.deltaTime * menuSpeed);

        if (HP <= -0.01f) HP = -0.01f;

        if (end)
        {
            playerController.transform.position = startPosition;
            return;
        }

        if (HP <= -0.01f)
        {
            Death();
            playerController.transform.position = startPosition;
            return;
        }

        HP -= Time.deltaTime * (hpLossSpeed + AdditionalHPLossSpeed);
    }
}
