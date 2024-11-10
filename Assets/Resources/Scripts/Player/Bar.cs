using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar: MonoBehaviour
{
    [SerializeField] protected Transform bar;
    [SerializeField] protected Vector3 dissapearDistance;
    [SerializeField] protected float speed;

    protected Vector3 startPos;
    protected float startWidth;
    protected float startCount;

    public bool Active { get; set; }
    public float Count { get; set; }

    public virtual void Start()
    {
        startPos = transform.localPosition;
        startWidth = bar.localScale.x;
        startCount = Count;
    }

    public virtual void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos + (Active ? Vector3.zero : dissapearDistance), Time.deltaTime * speed);
        bar.localScale = new Vector3(startWidth / startCount * Count, bar.localScale.y, bar.localScale.z);

        if (bar.localScale.x < 0f)
        {
            bar.localScale = new Vector3(0, bar.transform.localScale.y);
        }
    }
}
