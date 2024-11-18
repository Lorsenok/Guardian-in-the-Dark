using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public bool IsOn { get; set; } = true;

    [SerializeField] private GameObject checkmarkTrue;
    [SerializeField] private GameObject checkmarkFalse;

    private bool isMouseOver = false;

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void Update()
    {
        checkmarkTrue.SetActive(IsOn);
        checkmarkFalse.SetActive(!IsOn);

        if (isMouseOver && Input.GetMouseButtonDown(0) && GameButton.CanBeToched)
        {
            IsOn = !IsOn;
        }
    }
}
