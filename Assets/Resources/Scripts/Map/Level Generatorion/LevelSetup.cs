using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static LevelSetup Instance { get; private set; }

    [Header("Templates")]
    [SerializeField] private GameObject hub;
    [SerializeField] private LevelTemplatesVariation[] templates;
    public LevelTemplatesVariation EndTemplates;

    [Header("Setup")]
    [SerializeField] private int maxSpawnCount = 999;
    [SerializeField] private int defaultSpawnCount;
    public float SpawnTime = 1.0f;

    [Header("After Setup")]
    [SerializeField] private GameObject[] enableObjects;

    private GameObject lastTemplate;

    private int curSpawnID = 0;

    private int curJoints = 0;

    private void OnJointSpawn()
    {
        curJoints++;
    }

    private void OnJointWork()
    {
        curJoints--;
    }

    public bool CheckTemplateVariation(LevelJoint joint)
    {
        for (int i = 0; i < templates.Length; i++)
        {
            if (templates[i] == joint.Templates)
            {
                return true;
            }
        }
        return false;
    }

    public List<GameObject> CheckTemplates(LevelJoint joint)
    {
        List<GameObject> ranTemplates = new List<GameObject>();

        foreach (GameObject template in joint.Templates.Templates)
        {
            LevelTemplate temp = template.GetComponent<LevelTemplate>();
            if (temp.Direction == joint.Direction && temp.SpawnCount > 0 && template != lastTemplate)
            {
                ranTemplates.Add(template);
            }

            lastTemplate = template;
        }

        return ranTemplates;
    }

    public LevelTemplate SpawnTemplate(LevelJoint joint, List<GameObject> templates)
    {
        if (templates.Count == 0) return null;

        int rand = Random.Range(0, templates.Count - 1);

        GameObject obj = Instantiate(templates[rand], joint.transform.position, templates[rand].transform.rotation);

        obj.TryGetComponent(out LevelTemplate _temp);
        _temp.SpawnID = curSpawnID;
        _temp.SourceJoint = joint;

        curSpawnID++;

        templates[rand].TryGetComponent(out LevelTemplate temp);
        if (temp.SpawnCount < maxSpawnCount) temp.SpawnCount--;

        return _temp;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Debug.LogWarning("There is can not be more then 1 level setups");

            return;
        }

        Instance = this;

        LevelTemplate.OnJointSpawn += OnJointSpawn;
        LevelTemplate.OnJointWork += OnJointWork;
    }

    private void Start()
    {
        foreach (LevelTemplatesVariation l in  templates)
        {
            foreach (GameObject obj in l.Templates)
            {
                obj.TryGetComponent(out LevelTemplate temp);
                if (temp.SpawnCount < maxSpawnCount) temp.SpawnCount = defaultSpawnCount;
            }
        }

        Instantiate(hub);
    }

    private void Update()
    {
        if (curJoints <= 0)
        {
            if (enableObjects != null)
            {
                foreach (GameObject obj in enableObjects)
                {
                    obj.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
    }
}
