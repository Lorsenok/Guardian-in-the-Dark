using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    public void Awake()
    {
        Instance = this;
    }

    public void MenuOpen(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == menuName) { menus[i].ChangeState(true); }
            else if (menus[i].Open) { menus[i].ChangeState(false); }
        }
    }
}
