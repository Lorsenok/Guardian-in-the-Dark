using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string MenuName;
    public bool Open;

    [SerializeField] private float speed;
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 closePosition;

    public void ChangeState(bool state)
    {
        Open = state;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Open ? openPosition : closePosition, Time.deltaTime * speed);
    }
}
