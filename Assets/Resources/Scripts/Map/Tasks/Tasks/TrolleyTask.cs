using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyTask : Task
{
    private bool end = false;

    private void OnEnd()
    {
        end = true;
    }

    private void OnEnable()
    {
        RailsEnd.OnEnd += OnEnd;
    }

    private void OnDisable()
    {
        RailsEnd.OnEnd -= OnEnd;
    }

    public override bool Check()
    {
        return end;
    }
}
