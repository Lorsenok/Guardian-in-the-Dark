using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public string Name;

    public virtual bool Check()
    {
        return false;
    }
}
