using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTask : Task
{
    public override bool Check()
    {
        return HubArea.IsOnPlayer;
    }
}
