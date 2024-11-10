using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyActor : Enemy
{
    public static List<SpiderEnemyActor> fakeSpiders = new List<SpiderEnemyActor>();

    [Header("Spider")]
    [SerializeField] private bool isFake;
    [SerializeField] private Transform[] legs;
    [SerializeField] private Vector3 rotationInterval;

    private List<Vector3> startLegsRotation = new List<Vector3>();

    private bool isHitPlayer = false;

    public override void Start()
    {
        foreach (Transform t in legs)
        {
            startLegsRotation.Add(t.localEulerAngles);
        }

        if (isFake) fakeSpiders.Add(this);
        base.Start();
    }

    public override void OnDestroy()
    {
        if (!isFake) base.OnDestroy();
    }

    public override void GetDamage(int damage)
    {
        if (isFake) return;
        
        hp -= damage;
    }

    public override void Die(bool fromWeapon = true)
    {
        if (!isFake)
        {
            foreach (SpiderEnemyActor actor in fakeSpiders)
            {
                actor.Die(fromWeapon);
            }
        }
        base.Die(fromWeapon);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player) isHitPlayer = true;
        base.OnCollisionEnter2D(collision);
    }

    public override void Follow(Transform player)
    {
        if (!isHitPlayer) ProjMath.MoveTowardsAngle(transform, transform.eulerAngles.z * -1 + additionalAngle, speed * Time.deltaTime); 
    }

    public override void Update()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            legs[i].localRotation = Quaternion.Euler(startLegsRotation[i].x + Mathf.Sin(Time.timeSinceLevelLoad * rotationInterval.x),
                startLegsRotation[i].y + Mathf.Sin(Time.timeSinceLevelLoad * rotationInterval.y),
                startLegsRotation[i].z + Mathf.Sin(Time.timeSinceLevelLoad * rotationInterval.z));
        }

        base.Update();
    }
}
