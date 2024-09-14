using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance { get; private set; }
    public static List<GameObject> SpawnObjectsOnStart { get; set; } = new List<GameObject>();

    [SerializeField] private Image _image;
    [SerializeField] private float _speed = 0.7f;

    private float _opacityTarget = 0;

    private int _newSceneIndexId = -1;
    private string _newSceneIndexName;

    private void Awake()
    {
        Instance = this;

        foreach (GameObject obj in SpawnObjectsOnStart)
        {
            Instantiate(obj);
        }

        SpawnObjectsOnStart.Clear();
    }

    public void ChangeScene(int index)
    {
        _newSceneIndexId = index;
        _opacityTarget = 1;
    }

    public void ChangeScene(string index)
    {
        _newSceneIndexName = index;
        _opacityTarget = 1;
    }

    private void Update()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Mathf.MoveTowards(_image.color.a, _opacityTarget, _speed * Time.deltaTime));

        if (_image.color.a >= 1)
        {
            _opacityTarget = 0;
            if (_newSceneIndexId == -1)
            {
                SceneManager.LoadScene(_newSceneIndexName);
            }
            else
            {
                SceneManager.LoadScene(_newSceneIndexId);
            }
        }
    }
}
