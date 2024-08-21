using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject textPrefab;

    [SerializeField] private float hideDistance;
    private float startPosX;

    private bool isTaskShowing = false;

    [SerializeField] private float updateHideTimeSet;
    private float updateHideTime;

    [SerializeField] private float updateShowTimeSet;
    private float updateShowTime;

    [SerializeField] private Transform[] additionalPanels;

    [Header("Visuals")]
    [SerializeField] private float textDissapearDelaySet;
    [SerializeField] private float textColoringSpeed;
    [SerializeField] private Color textColorSet;

    private float textDissapearDelay;

    private List<GameObject> texts = new();
    private List<Task> tasks = new();

    private TextMeshProUGUI curDissapearingText;

    private bool isOnUpdate = false;
    private bool isOnUpdateEnd = false;

    private bool showAtEnd = false;

    private void Start()
    {
        startPosX = transform.position.x;
        textDissapearDelay = textDissapearDelaySet;
    }

    public void UpdateTasks(List<Task> tasks, bool atOnce = false, bool showAtEnd = false)
    {
        this.showAtEnd = showAtEnd;
        updateShowTime = updateShowTimeSet;

        isOnUpdate = true;
        isOnUpdateEnd = false;
        updateHideTime = 0;

        if (this.tasks != null)
        {
            foreach (Task task in this.tasks)
            {
                if (!tasks.Contains(task))
                {
                    foreach (GameObject text in texts)
                    {
                        if (text.GetComponent<TextMeshProUGUI>().text == task.Name)
                        {
                            curDissapearingText = text.GetComponent<TextMeshProUGUI>();
                        }
                    }
                }
            }
        }

        this.tasks.Clear();

        foreach (Task task in tasks)
        {
            this.tasks.Add(task);
        }

        if (atOnce)
        {
            TextUpdate();
        }
    }

    private void TextUpdate()
    {
        if (texts != null)
        {
            foreach (GameObject text in texts)
            {
                Destroy(text);
            }

            texts.Clear();
        }

        foreach (Task task in tasks)
        {
            TextMeshProUGUI text = Instantiate(textPrefab, transform).GetComponent<TextMeshProUGUI>();
            text.text = task.Name;
            texts.Add(text.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && updateHideTime <= 0) isTaskShowing = true;
        else if (!isOnUpdate && !isOnUpdateEnd && updateShowTime <= 0) isTaskShowing = false;

        if (isTaskShowing) transform.position = Vector3.Lerp(transform.position, new Vector3(startPosX, transform.position.y, 0), Time.deltaTime * speed * 2);
        else transform.position = Vector3.Lerp(transform.position, new Vector3(startPosX + hideDistance, transform.position.y, 0), Time.deltaTime * speed / 2);

        if (updateShowTime > 0)
        {
            if (updateShowTime < updateShowTimeSet / 1.5f && curDissapearingText != null)
            {
                textDissapearDelay -= Time.deltaTime;

                if (textDissapearDelay <= 0)
                {
                    textDissapearDelay = textDissapearDelaySet;

                    if (curDissapearingText.text.Length > 0)
                    {
                        char[] sim = new char[curDissapearingText.text.Length - 1];
                        for (int i = 0; i < sim.Length; i++)
                        {
                            sim[i] = curDissapearingText.text[i];
                        }
                        curDissapearingText.text = new(sim);
                    }
                }

                curDissapearingText.color = Color.Lerp(curDissapearingText.color, textColorSet, Time.deltaTime * textColoringSpeed);
            }


            updateShowTime -= Time.deltaTime;
            isTaskShowing = true;
        }

        if (updateHideTime > 0)
        {
            updateHideTime -= Time.deltaTime;
            isTaskShowing = false;
        }

        if (updateShowTime <= 0 && isOnUpdate)
        {
            isOnUpdate = false;
            isOnUpdateEnd = true;
            updateHideTime = updateHideTimeSet;
        }

        if (updateHideTime <= 0 && isOnUpdateEnd)
        {
            TextUpdate();
            isOnUpdateEnd = false;

            if (showAtEnd) updateShowTime = updateShowTimeSet * 1.5f;
        }

        if (additionalPanels != null)
        {
            foreach (Transform panel in  additionalPanels)
            {
                panel.position = new(transform.position.x, panel.position.y);
            }
        }
    }
}
