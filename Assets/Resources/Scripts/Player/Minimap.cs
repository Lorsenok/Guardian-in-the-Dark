using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform offset;
    [SerializeField] private Transform upperOffset;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject specialPointPrefab;

    [SerializeField] private Transform player;
    [SerializeField] private Transform hub;

    [SerializeField] private float deviderOn;
    [SerializeField] private float deviderOff;

    [SerializeField] private float distance;

    [SerializeField] private float positionSetSpeed;
    [SerializeField] private float offsetSpeed;

    [SerializeField] private float minAlpha;

    private List<Vector3> points = new List<Vector3>();
    private List<DisolvingObject> pointsDisolve = new List<DisolvingObject>();

    private List<Transform> specialPoints = new List<Transform>();
    private List<Vector3> specialPointsPositions  = new List<Vector3>();

    private int curIndex = 0;

    private float GetCurMapDevider()
    {
        return Input.GetKey(KeyCode.Tab) ? deviderOn : deviderOff;
    }

    private void SpawnPoint(Vector3 pos, bool hasBacklit)
    {
        if (hasBacklit) return;

        GameObject obj = Instantiate(pointPrefab, offset);
        obj.transform.localPosition = PlayerManager.Instance.GetPlayerPosition().position / GetCurMapDevider();
        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0f);

        int i = 0;
        foreach (Vector3 objs in points)
        {
            if (Vector2.Distance(objs, pos) < distance)
            {
                if (pointsDisolve[i].CurAlpha > minAlpha)
                {
                    Destroy(obj);
                    return;
                }
            }

            i++;
        }

        DisolvingObject disObj = obj.GetComponent<DisolvingObject>();
        disObj.Index = curIndex;
        curIndex++;

        points.Add(pos);
        pointsDisolve.Add(disObj);
    }

    private void SpawnSpecialPoint(Vector3 pos, bool isAlowed)
    {
        if (!isAlowed) return;

        specialPoints.Add(Instantiate(specialPointPrefab, upperOffset).transform);
        specialPointsPositions.Add(pos);

        specialPoints[specialPoints.Count - 1].localPosition = PlayerManager.Instance.GetPlayerPosition().position / GetCurMapDevider();
    }

    private void Awake()
    {
        Lidar.OnPointSpawn += SpawnPoint;
        Backlit.OnAppear += SpawnSpecialPoint;
        Backlit3D.OnAppear += SpawnSpecialPoint;
    }

    private void Update()
    {
        offset.localPosition = Vector3.Lerp(offset.localPosition, 
            -PlayerManager.Instance.GetPlayerPosition().position / GetCurMapDevider(),
            Time.deltaTime * offsetSpeed);

        player.localPosition = Vector3.Lerp(player.localPosition,
                PlayerManager.Instance.GetPlayerPosition().position / GetCurMapDevider(),
                positionSetSpeed * Time.deltaTime);

        hub.transform.position = offset.position;
        upperOffset.transform.position = offset.position;

        List<int> objsRemove = new();

        for (int i = 0; i < pointsDisolve.Count; i++)
        {
            if (pointsDisolve[i] == null)
            {
                objsRemove.Add(i);
                continue;
            }

            pointsDisolve[i].transform.localPosition = Vector3.Lerp(pointsDisolve[i].transform.localPosition, 
                points[i] / GetCurMapDevider(), 
                positionSetSpeed * Time.deltaTime);
        }

        for (int i = 0; i < specialPoints.Count; i++)
        {
            specialPoints[i].localPosition = Vector3.Lerp(specialPoints[i].localPosition,
                specialPointsPositions[i] / GetCurMapDevider(),
                positionSetSpeed * Time.deltaTime);
        }

        foreach (int obj in objsRemove)
        {
            pointsDisolve.RemoveAt(obj);
            points.RemoveAt(obj);
        }
    }
}
