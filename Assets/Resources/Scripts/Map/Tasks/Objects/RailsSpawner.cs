using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsSpawner : MonoBehaviour
{
    private Rails lastRails;

    private Vector2 startDir;

    private bool canWork = false;

    [SerializeField] private AIPath pathfinding;

    [SerializeField] private GameObject railsPrefab;
    [SerializeField] private GameObject railsRotatePrefab;

    [SerializeField] private GameObject railsEnd;

    [SerializeField] private GameObject leverPrefab;
    [SerializeField] private GameObject wirePrefab;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnAdditionalRotation;
    [SerializeField] private int leverRailsQuantity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canWork)
        {
            if (collision.gameObject.TryGetComponent(out LevelTemplate temp))
            {
                if (temp.TaskObjectSpawners.Count > 0)
                {
                    Instantiate(leverPrefab, temp.TaskObjectSpawners[0].transform.position, leverPrefab.transform.rotation).TryGetComponent(out Lever lever);
                    Destroy(temp.TaskObjectSpawners[0]);
                    Vector3 leverPos = lever.transform.position;
                    
                    Transform wireTrans = Instantiate(wirePrefab, (leverPos + lastRails.transform.position) / 2, Quaternion.Euler(0, 0, ProjMath.RotateTowardsPosition(leverPos, lastRails.transform.position))).transform;

                    wireTrans.localScale = new(Vector2.Distance(leverPos, lastRails.transform.position), wireTrans.localScale.y);

                    Rails irails = lastRails;
                    for (int i = 0; i < leverRailsQuantity; i++)
                    {
                        lever.Rails.Add(irails); 
                        irails = lastRails.LastRails;
                    }
                }
            }

            return;
        }

        if (collision.gameObject.TryGetComponent(out LevelTemplate template))
        {
            startDir = -template.Direction;
            canWork = true;
        }
    }

    private void Spawn(Vector2 dir, bool rotate = false, Vector3 position = new())
    {
        Rails obj;

        Vector3 pos = position == Vector3.zero ? transform.position : position;

        if (rotate) Instantiate(railsRotatePrefab, pos, Quaternion.Euler(0, 0, ProjMath.RotateTowardsPosition(pos, pos + new Vector3(dir.x, dir.y, 0)))).TryGetComponent(out obj);
        else Instantiate(railsPrefab, pos, Quaternion.Euler(0, 0, ProjMath.RotateTowardsPosition(pos, pos + new Vector3(dir.x, dir.y, 0)) + spawnAdditionalRotation)).TryGetComponent(out obj);

        obj.LastRails = lastRails;

        lastRails = obj;
        obj.Direction = dir;
    }

    private void Start()
    {
        pathfinding.destination = HubArea.Instance.GetComponent<LevelTemplate>().Joints[0].transform.position;
    }

    private bool onSpawnedStartRails = false;
    private void Update()
    {
        if (!canWork) return;

        if (Vector2.Distance(transform.position, pathfinding.destination) < 1)
        {
            Instantiate(railsEnd, transform.position, railsEnd.transform.rotation);
            Destroy(gameObject);
        }

        if (!onSpawnedStartRails)
        {
            Spawn(startDir);
            onSpawnedStartRails = true;
        }

        bool spawn = false;
        Vector2 dir = Vector2.zero;

        float x = transform.position.x - lastRails.transform.position.x;
        float y = transform.position.y - lastRails.transform.position.y;

        if (y > spawnDistance || y < -spawnDistance)
        {
            dir = transform.position.y > lastRails.transform.position.y ? Vector2.up : Vector2.down;
            spawn = true;
        }

        if (x > spawnDistance || x < -spawnDistance)
        {
            dir = transform.position.x > lastRails.transform.position.x ? Vector2.right : Vector2.left;
            spawn = true;
        }

        if (spawn)
        {
            Spawn(dir, lastRails.Direction != dir, lastRails.transform.position + new Vector3(lastRails.Direction.x, lastRails.Direction.y, 0) * spawnDistance);
        }
    }
}
