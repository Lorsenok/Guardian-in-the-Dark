using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private AudioSource deathsound;

    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 playerSpawnPosition;

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

    [SerializeField] private float dieTime;
    [SerializeField] private string dieSceneIndex;
    private bool hasDied = false;

    private bool end = false;

    public bool IsMenuClosed { get; set; } = true;
    private Vector3 startMenuPos;
    [SerializeField] private float menuSpeed;
    [SerializeField] private Vector2 menuCloseDistance;

    [SerializeField] private GameObject enemyManager;

    [SerializeField] private float gameTimeReturnSpeed = 1.0f;

    public bool IsGameTimeChanging { get; set; } = false; 

    public bool IsDeadByEnemy { get; set; } = false;

    private void Awake()
    {
        HP = Config.PlayerHP;
        Instance = this;
        playerController = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity).GetComponent<Controller>();

        startMenuPos = menu.transform.localPosition;
        menu.transform.position = startMenuPos + (Vector3)menuCloseDistance;
    }

    private void Start()
    {
        if (enemyManager != null) Instantiate(enemyManager);

        playerMaterial.SetFloat("_Disolve", 0f);
        playerMaterial.SetFloat("_Smoothness", 0.5f);
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
            deathsound.volume = Config.Sound;
            deathsound.Play();
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

    private void Update()
    {
        if (!IsGameTimeChanging) Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, Time.deltaTime * gameTimeReturnSpeed);

        if (Input.GetKeyDown(KeyCode.Escape)) IsMenuClosed = !IsMenuClosed;

        menu.transform.localPosition = Vector3.Lerp(menu.transform.localPosition, startMenuPos + (IsMenuClosed ? menuCloseDistance : Vector3.zero), Time.deltaTime * menuSpeed);

        if (HP < -0.01f) HP = -0.01f;

        if (end)
        {
            playerController.transform.position = startPosition;

            dieTime -= Time.deltaTime;
            if (dieTime <= 0 && !hasDied)
            {
                hasDied = true;
                SceneSwitcher.Instance.ChangeScene(dieSceneIndex);
            }

            return;
        }

        if (HP <= -0.01f)
        {
            if (!IsDeadByEnemy) SceneSwitcher.Instance.ChangeScene("Die");
            else Death();
            return;
        }

        HP -= Time.deltaTime * (hpLossSpeed + AdditionalHPLossSpeed);
    }
}
