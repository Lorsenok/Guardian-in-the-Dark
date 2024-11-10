using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected float AdditionalLoadingTime = 5f;

    public static Action OnComplete { get; set; }

    public string Name;

    private bool hasCompleted = false;

    public virtual bool Check()
    {
        return false;
    }

    public virtual void Update()
    {
        if (Time.deltaTime < 1f) AdditionalLoadingTime -= Time.deltaTime;
        if (AdditionalLoadingTime <= 0 && !hasCompleted)
        {
            OnComplete?.Invoke();
            hasCompleted = true;
        }
    }
}
