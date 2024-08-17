using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskUI grid;
    [SerializeField] private Task endTask;

    private List<Task> tasks = new List<Task>();

    private bool isEnd = false;

    [SerializeField] private float showTaskTime;

    private void Start()
    {
        foreach (Task task in GetComponentsInChildren<Task>())
        {
            if (task == endTask) continue;
            tasks.Add(task);
        }
        grid.UpdateTasks(tasks, atOnce:true);
    }

    private void Update()
    {
        Task curTask = null;

        if (tasks.Count > 0)
        {
            foreach (Task task in tasks)
            {
                if (task.Check())
                {
                    curTask = task;
                }
            }
        }

        if (tasks.Count == 0 && curTask == null && !isEnd)
        {
            tasks.Add(endTask);
            grid.UpdateTasks(tasks, showAtEnd:true);
            isEnd = true;
        }

        if (curTask != null)
        {
            tasks.Remove(curTask);
            grid.UpdateTasks(tasks);

            curTask = null;
        }
    }
}
