using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsEnd : MonoBehaviour
{
    public static Action OnEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Trolley trolley))
        {
            trolley.IsWork = false;
            OnEnd?.Invoke();
        }
    }
}
