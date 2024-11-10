using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    public static Action OnJointSpawn;
    public static Action OnJointWork;

    public bool Destroyable { get; set; } = true;

    public int SpawnCount;
    public int SpawnID { get; set; }

    public LevelJoint[] Joints;
    public Vector2 Direction;

    private float spawnTime = 0f;
    private bool hasWorked = false;

    public LevelJoint SourceJoint { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Destroyable) return;

        if (collision.TryGetComponent(out LevelTemplate template))
        {
            if (template.SpawnID < SpawnID && template.Destroyable)
            {
                SourceJoint.Templates = LevelSetup.Instance.EndTemplates;

                if (LevelSetup.Instance.CheckTemplateVariation(SourceJoint))
                {
                    LevelSetup.Instance.SpawnTemplate(SourceJoint, LevelSetup.Instance.CheckTemplates(SourceJoint)).Destroyable = false;
                }
                Destroy(gameObject);
            }
        }
    }

    public List<AdditionalTaskObjectSpawner> TaskObjectSpawners { get; set; } = new();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AdditionalTaskObjectSpawner spawner))
        {
            if (!TaskObjectSpawners.Contains(spawner)) TaskObjectSpawners.Add(spawner);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AdditionalTaskObjectSpawner spawner))
        {
            if (TaskObjectSpawners.Contains(spawner)) TaskObjectSpawners.Remove(spawner);
        }
    }

    private void Start()
    {
        for (int i = 0; i < Joints.Length; i++)
        {
            OnJointSpawn.Invoke();
        }

        spawnTime = LevelSetup.Instance.SpawnTime;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < Joints.Length; i++)
        {
            OnJointWork.Invoke();
        }
    }

    public LevelTemplate Spawn(LevelJoint joint)
    {
        OnJointWork.Invoke();

        if (LevelSetup.Instance.CheckTemplateVariation(joint))
        {
            hasWorked = true;

            LevelTemplate obj = LevelSetup.Instance.SpawnTemplate(joint, LevelSetup.Instance.CheckTemplates(joint));

            return obj;
        }

        return null;
    }

    private void Update()
    {
        if (spawnTime > 0) spawnTime -= Time.deltaTime;

        if (Joints.Length == 0 || spawnTime > 0 || hasWorked) return;

        foreach (LevelJoint joint in Joints)
        {
            Spawn(joint);
        }
    }
}
