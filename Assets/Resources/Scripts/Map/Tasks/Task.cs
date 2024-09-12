using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected float AdditionalLoadingTime = 5f;

    public static Action OnComplete { get; set; }

    public string Name;

    public virtual bool Check()
    {
        return false;
    }

    public virtual void Update()
    {
        AdditionalLoadingTime -= Time.deltaTime;
        if (AdditionalLoadingTime <= 0)
        {
            OnComplete?.Invoke();
            enabled = false;
        }
    }
}
