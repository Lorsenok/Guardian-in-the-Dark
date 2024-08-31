using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJoint : MonoBehaviour
{
    public bool HasUsed { get; set; } = false;

    public LevelTemplatesVariation Templates;
    public Vector2 Direction;
}
