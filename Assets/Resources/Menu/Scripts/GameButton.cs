using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonFunction
{
    Nothing,
    Start,
    Exit,
    MenuOpen,
    MenuClose
}

public class GameButton : MonoBehaviour
{
    public static bool CanBeToched { get; set; } = true;

    [Header("Tech")]
    [SerializeField] protected int mouseButton;
    [SerializeField] protected ButtonFunction OnClick;
    [SerializeField] protected string index;

    [Header("Animations")]
    [SerializeField] protected Animator animator;

    [SerializeField] protected string IsClicking;
    [SerializeField] protected string IsPointing;
    [SerializeField] protected string IsNotPointing;

    [SerializeField] protected float moveDistance;
    [SerializeField] protected Vector3 moveDirection;
    [SerializeField] protected float moveSpeed;

    protected bool isMousePointing = false;

    protected Vector3 startPos;

    public virtual void OnMouseOver()
    {
        isMousePointing = true;
    }

    public virtual void OnMouseExit()
    {
        isMousePointing = false;
    }

    public virtual void Awake()
    {
        startPos = transform.localPosition;
    }

    public virtual void Update()
    {
        if (!CanBeToched)
        {
            isMousePointing = false;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, isMousePointing ? startPos + moveDirection * moveDistance / (Input.GetMouseButton(mouseButton) ? 2 : 1) : startPos, Time.deltaTime * moveSpeed);

        animator.SetBool(IsPointing, isMousePointing && !Input.GetMouseButton(mouseButton));
        animator.SetBool(IsClicking, isMousePointing && Input.GetMouseButton(mouseButton));
        animator.SetBool(IsNotPointing, !isMousePointing);

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
            }
        }
    }
}
