using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonFunction
{
    Nothing,
    Start,
    Exit,
    MenuOpen,
    MenuClose,
    DeleteAllData,
    DefaultSettings
}

public class GameButton : MonoBehaviour
{
    public static bool CanBeToched { get; set; } = true;

    [Header("Tech")]
    [SerializeField] protected int mouseButton;
    [SerializeField] protected ButtonFunction OnClick;
    [SerializeField] protected string index;
    [SerializeField] protected int ignoreSoundsSceneId = 1;

    [Header("Animations")]
    [SerializeField] protected Animator animator;

    [SerializeField] protected string IsClicking;
    [SerializeField] protected string IsPointing;
    [SerializeField] protected string IsNotPointing;

    [SerializeField] protected float moveDistance;
    [SerializeField] protected Vector3 moveDirection;
    [SerializeField] protected float moveSpeed;

    [Header("Sounds")]
    [SerializeField] private AudioSource overSound;
    private bool onOverSound = true;
    [SerializeField] private AudioSource clickSound;
    private bool onClickSound = true;

    protected bool isMousePointing = false;

    protected Vector3 startPos;

    private bool ignoreSounds = true;

    public virtual void OnMouseOver()
    {
        isMousePointing = true;

        if (onOverSound && CanBeToched && !ignoreSounds)
        {
            overSound.Play();
            onOverSound = false;
        }
    }

    public virtual void OnMouseExit()
    {
        isMousePointing = false;

        onOverSound = true;
    }

    public virtual void Awake()
    {
        CanBeToched = true;
        startPos = transform.localPosition;
        ignoreSounds = SceneManager.GetActiveScene().buildIndex == ignoreSoundsSceneId;
    }

    public virtual void Update()
    {
        if (!ignoreSounds)
        {
            overSound.volume = Config.Sound;
            clickSound.volume = Config.Sound;
        }

        if (!CanBeToched)
        {
            isMousePointing = false;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, isMousePointing ? startPos + moveDirection * moveDistance / (Input.GetMouseButton(mouseButton) ? 2 : 1) : startPos, Time.deltaTime * moveSpeed);

        animator.SetBool(IsPointing, isMousePointing && !Input.GetMouseButton(mouseButton));
        animator.SetBool(IsClicking, isMousePointing && Input.GetMouseButton(mouseButton));
        animator.SetBool(IsNotPointing, !isMousePointing);

        if (isMousePointing && Input.GetMouseButtonDown(mouseButton))
        {
            onClickSound = false;
        }

        if (isMousePointing && Input.GetMouseButtonUp(mouseButton))
        {
            switch (OnClick)
            {
                case ButtonFunction.Start:
                    SceneSwitcher.Instance.ChangeScene(index);
                    break;

                case ButtonFunction.Exit:
                    Application.Quit();
                    break;

                case ButtonFunction.MenuOpen:
                    MenuManager.Instance.MenuOpen(index);
                    break;

                case ButtonFunction.MenuClose:
                    PlayerManager.Instance.IsMenuClosed = true;
                    break;
                case ButtonFunction.DeleteAllData:
                    PlayerPrefs.DeleteAll();
                    Config.Money = Config.StartMoney;
                    Config.Progress = 0;
                    SceneSwitcher.Instance.ChangeScene(index);
                    CanBeToched = false;
                    break;
                case ButtonFunction.DefaultSettings:
                    SettingsSetup.Instance.ClearSettings();
                    break;
            }
        }


        if (!onClickSound && !ignoreSounds)
        {
            clickSound.Play();
            onClickSound = true;
        }
    }
}
