using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableObject : MonoBehaviour
{
    protected bool canBeTaked = false;

    protected Controller player;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Controller>(out Controller _player))
        {
            canBeTaked = true;
            player = _player;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Controller>(out Controller _player))
        {
            canBeTaked = false;
            player = _player;
        }
    }
}
