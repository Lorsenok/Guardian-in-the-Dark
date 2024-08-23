using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; private set; }

    public static bool CanMove { get; set; } = true;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    public float CurrectSpeed { get; set; }

    [SerializeField] private float additionalRotate;
    private float speedMultiplier;

    private Rigidbody2D rg;

    private void Start()
    {
        speedMultiplier = Config.SpeedMultiplier;
        CurrectSpeed = speed;

        rg = GetComponent<Rigidbody2D>();

        Instance = this;
    }

    private void Move() // It looks a little bit strange, maybe i need to fix it then
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 normalizedMovement = new Vector2(Mathf.Abs(movement.x), Mathf.Abs(movement.y)).normalized;
        rg.velocity = movement * normalizedMovement * CurrectSpeed * speedMultiplier;
    }

    private void Rotate()
    {
        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotateZ + additionalRotate), Time.deltaTime * speed / 2f * speedMultiplier);
    }

    private void Update()
    {
        if (!CanMove)
        {
            rg.velocity = Vector2.zero;
            return;
        }
        CurrectSpeed = Mathf.Lerp(CurrectSpeed, speed, Time.deltaTime * acceleration);
        Rotate();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        Move();
    }

}
