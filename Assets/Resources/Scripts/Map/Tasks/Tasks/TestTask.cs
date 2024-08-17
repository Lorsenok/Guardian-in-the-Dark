using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TestTask : Task
{
    [SerializeField] private KeyCode code;

    public override bool Check()
    {
        return Input.GetKeyDown(code);
    }
}
