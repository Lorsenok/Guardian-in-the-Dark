using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EndTask : Task
{
    [SerializeField] private GameObject completeObject;
    [SerializeField] private int sceneID = 0;

    public override bool Check()
    {
        if (HubArea.IsOnPlayer)
        {
            SceneSwitcher.SpawnObjectsOnStart.Add(completeObject);
            SceneSwitcher.Instance.ChangeScene(Config.HasEnd ? Config.EndSceneID : sceneID);
            enabled = false;
        }
        return HubArea.IsOnPlayer;
    }
}
