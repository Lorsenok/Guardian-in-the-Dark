using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
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

    private void Start()
    {
        spawnTime = LevelSetup.Instance.SpawnTime;
    }

    public LevelTemplate Spawn(LevelJoint joint, LevelTemplate lastTemplate)
    {
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
            Spawn(joint, this);
        }
    }
}
