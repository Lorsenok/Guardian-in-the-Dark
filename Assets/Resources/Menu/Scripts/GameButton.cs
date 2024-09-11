using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonFunction
{
    Start,
    Exit,
    MenuOpen,
    MenuClose
}

public class GameButton : MonoBehaviour
{


    [Header("Tech")]
    [SerializeField] private int mouseButton;
    [SerializeField] private ButtonFunction OnClick;
    [SerializeField] private string index;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [SerializeField] private string IsClicking;
    [SerializeField] private string IsPointing;
    [SerializeField] private string IsNotPointing;

    [SerializeField] private float moveDistance;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    private bool isMousePointing = false;

    private Vector3 startPos;

    public void OnMouseOver()
    {
        isMousePointing = true;
    }

    public void OnMouseExit()
    {
        isMousePointing = false;
    }

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
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
                    GetComponentInParent<MenuManager>().gameObject.SetActive(false);
                    break;
            }
        }
    }
}
