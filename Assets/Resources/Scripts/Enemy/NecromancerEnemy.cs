using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NecromancerEnemy : Enemy
{

    [Header("Necromancer")]
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private GameObject point;
    [SerializeField] private float minToPointDistance;
    [SerializeField] private float rotateSpeed;

    Transform curpoint;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2 * damage * damage;

        while (Vector2.Distance(curpoint.position, PlayerManager.Instance.GetPlayerPosition().position) < minDistance)
        {
            curpoint.position = new(Random.Range(-maxDistance, maxDistance) + transform.position.x, Random.Range(-maxDistance, maxDistance) + transform.position.y);
        }

        player = curpoint;
    }

    public override void Start()
    {
        base.Start();

        curpoint = Instantiate(point, transform.position, Quaternion.identity).transform;

        while (Vector2.Distance(curpoint.position, player.position) < minDistance)
        {
            curpoint.position = new(Random.Range(-maxDistance, maxDistance) + transform.position.x, Random.Range(-maxDistance, maxDistance) + transform.position.y);
        }

        player = curpoint;
    }

    public override void Follow(Transform player)
    {
        if (Vector2.Distance(transform.position, curpoint.position) <= minToPointDistance)
        {
            if (player == curpoint) this.player = PlayerManager.Instance.GetPlayerPosition();
            follow.enabled = false;
            SmoothRotateTowardsPosition(rotateSpeed, player.position);
        }
        else base.Follow(player);
    }
}
