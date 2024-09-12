using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] enableScripts;
    [SerializeField] private GameObject[] disableObjects;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textChangeSpeed;

    private bool hasCompleted = false;

    private void Work()
    {
        foreach (MonoBehaviour beh  in enableScripts)
        {
            beh.enabled = true;
        }
        foreach (GameObject obj in  disableObjects)
        {
            obj.SetActive(false);
        }

        hasCompleted = true;
    }

    private void OnEnable()
    {
        Task.OnComplete += Work;
    }

    private void OnDisable()
    {
        Task.OnComplete -= Work;
    }

    private void Update()
    {
        if (!hasCompleted) return;

        text.color = Color.Lerp(text.color, new Color(0, 0, 0, 0), Time.deltaTime * textChangeSpeed);
    }
}
