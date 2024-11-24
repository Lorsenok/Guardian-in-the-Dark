using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    [SerializeField] private AudioSource lidarWorkSound;
    [SerializeField] private GameObject soundObject;
    [SerializeField] private float soundMultiplier;

    [Header("Lidar setup")]
    [SerializeField] private float speedS;
    [SerializeField] private float speedByScrollMultiplier;
    [SerializeField] private float additionalScrollDifference;
    [SerializeField] private float speedDecreaser;
    [SerializeField] private float minDifference;
    [SerializeField] private float maxDifference;
    [SerializeField] private float maxDistance;
    [SerializeField] private float divisionsQuantity;
    [SerializeField] private float differenceChangeSpeed;
    [SerializeField] private float differenceMultiplier;

    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private LayerMask blockCollideLayer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform pointDirection;

    [SerializeField] private float minDistance;
    [SerializeField] private float minDistanceForPoints;

    [Header("Points Color")]
    [SerializeField] private List<Color> startColorList;
    [SerializeField] private float colorChangeSpeed;
    [SerializeField] private float colorRGBMin;
    [SerializeField] private float colorRGBMax;

    [Header("Ray setup")]
    [SerializeField] private GameObject pointRay;
    [SerializeField] private float rayWidth;

    [Header("Scaner setup")]
    [SerializeField] private Transform scanner;
    [SerializeField] private float scannerScaleYMultiplier;
    [SerializeField] private float scannerAppearSpeed;

    [Header("Lidar Bomb Setup")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombSpawnDelay;

    public float CurBombSpawnDelay { get; set; } = 0f;
    public float BombSpawnDelay { get; set; } = 0f;

    public bool IsWorking { get; set; } = true;
    public float TimeSinceLastRay { get; set; } = 0f;

    private SpriteRenderer scannerSpr;

    private float curSpeedS = 0f;

    private float delay = 0;

    private float currectDifference;
    private float currectDifferenceSet;
    private float differenceDistance;

    public static Action<Vector3, bool> OnPointSpawn { get; set; }

    private List<Vector3> points = new List<Vector3>();

    private float bombReloadSpeedMultiplier;
    private int pointsSpawnAtTheSameTime;

    private void Start()
    {
        bombReloadSpeedMultiplier = Config.BombReloadSpeedMultiplier;
        pointsSpawnAtTheSameTime = Config.RaysAtTheSameTime;

        scannerSpr = scanner.GetComponentInChildren<SpriteRenderer>();

        differenceDistance = maxDifference - minDifference;
        currectDifference = minDifference + differenceDistance;
        curPointColor = startColorList[UnityEngine.Random.Range(0, startColorList.Count)];
        currectDifferenceSet = minDifference + differenceDistance / 2;
        currectDifference = currectDifferenceSet;

        BombSpawnDelay = bombSpawnDelay;
    }

    public RaycastHit2D CurrectRay { get; set; }

    Color curPointColor;
    public void SpawnRay(Vector3 startPos, Vector3 dir, bool spawnRay = true)
    {
        TimeSinceLastRay = 0f;

        Vector3 direction = -(startPos - dir);
        float diff = currectDifference * differenceMultiplier;
        direction += new Vector3(UnityEngine.Random.Range(-diff, diff), UnityEngine.Random.Range(-diff, diff), 0);
        RaycastHit2D raycast = Physics2D.Raycast(startPos, direction.normalized, Mathf.Infinity, collideLayer);

        RaycastHit2D raycastBlock = Physics2D.Raycast(startPos, direction.normalized, Mathf.Infinity, blockCollideLayer);

        if (Vector2.Distance(startPos, raycast.point) > Vector2.Distance(startPos, raycastBlock.point)) return;

        CurrectRay = raycast;

        Vector3 pos = raycast.point;

        if (pos == Vector3.zero | Vector2.Distance(pos, transform.position) > maxDistance) return;


        //Spawn point and ray
        GameObject point = Instantiate(pointPrefab, pos, Quaternion.identity);
        point.GetComponent<SpriteRenderer>().color = curPointColor;

        if (Config.LidarPointSounds)
        {
            Instantiate(soundObject, point.transform.position, Quaternion.identity).TryGetComponent(out AudioSource sound);
            sound.volume = Config.Sound * soundMultiplier;
            sound.Play();
        }

        if (Vector2.Distance(point.transform.position, startPos) < minDistance)
        {
            Destroy(point);
            return;
        }

        if (curPointColor != Color.white)
        {
            int rgbChoose = UnityEngine.Random.Range(1, 3);

            if (rgbChoose == 1) curPointColor.r += colorChangeSpeed;
            if (rgbChoose == 2) curPointColor.g += colorChangeSpeed;
            if (rgbChoose == 3) curPointColor.b += colorChangeSpeed;

            if (curPointColor.r > colorRGBMax) curPointColor.r = colorRGBMin;
            if (curPointColor.g > colorRGBMax) curPointColor.g = colorRGBMin;
            if (curPointColor.b > colorRGBMax) curPointColor.b = colorRGBMin;
        }

        raycast.collider.gameObject.TryGetComponent(out Backlit backlit);
        raycast.collider.gameObject.TryGetComponent(out Backlit3D backlit3d);

        bool hasBacklit = false;

        if (backlit != null)
        {
            backlit.Light();
            point.GetComponent<SpriteRenderer>().color = backlit.color;

            hasBacklit = true;
        }
        if (backlit3d != null)
        {
            backlit3d.Light();
            point.GetComponent<SpriteRenderer>().color = backlit3d.color;

            hasBacklit = true;
        }

        Vector3 pointPos = point.transform.position;

        if (spawnRay)
        {
            Transform rayToPointStart = Instantiate(pointRay, dir, Quaternion.identity).transform;
            Transform rayToPoint = rayToPointStart.GetComponentInChildren<SpriteRenderer>().transform;


            Vector3 diference = pointPos - rayToPointStart.position;
            float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
            rayToPointStart.transform.rotation = Quaternion.Euler(0, 0, rotateZ);


            float distance = Vector2.Distance(rayToPointStart.position, pointPos);

            rayToPoint.localScale = new Vector2(distance, rayWidth);
            rayToPoint.localPosition = new Vector2(distance / 2, 0);
        }

        foreach (Vector3 p in points)
        {
            if (Vector2.Distance(p, pointPos) < minDistanceForPoints)
            {
                Destroy(point);
                return;
            }
        }

        points.Add(pointPos);

        OnPointSpawn?.Invoke(pointPos, hasBacklit);
    }

    private void Update()
    {
        if (!Controller.CanMove) return;

        TimeSinceLastRay += Time.deltaTime;

        curSpeedS = speedS + speedByScrollMultiplier / (currectDifference + 0.1f);

        //Shooting
        if (delay <= 0 & Input.GetMouseButton(0) && IsWorking && PlayerManager.Instance.IsMenuClosed)
        {
            delay = curSpeedS + currectDifference * speedDecreaser;
            for (int i = 0; i < pointsSpawnAtTheSameTime; i++) SpawnRay(transform.position, pointDirection.position);
        }
        if (delay > 0) delay -= Time.deltaTime;


        //Scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0) currectDifferenceSet -= differenceDistance / divisionsQuantity;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) currectDifferenceSet += differenceDistance / divisionsQuantity;
        currectDifferenceSet = Mathf.Clamp(currectDifferenceSet, minDifference, maxDifference);

        currectDifference = Mathf.Lerp(currectDifference, currectDifferenceSet, Time.deltaTime * differenceChangeSpeed);

        if (Input.GetMouseButton(0) && IsWorking && PlayerManager.Instance.IsMenuClosed)
        {
            scannerSpr.color = new Color(scannerSpr.color.r, scannerSpr.color.g, scannerSpr.color.b, Mathf.Lerp(scannerSpr.color.a, 1, Time.deltaTime * scannerAppearSpeed));
            if (!lidarWorkSound.isPlaying) lidarWorkSound.Play();
        }
        else
        {
            scannerSpr.color = new Color(scannerSpr.color.r, scannerSpr.color.g, scannerSpr.color.b, Mathf.Lerp(scannerSpr.color.a, 0, Time.deltaTime * scannerAppearSpeed));
            lidarWorkSound.Stop();
        }

        //Scanner
        if (Input.GetMouseButton(0))
        {
            scanner.localScale = new Vector2(maxDistance, currectDifference * scannerScaleYMultiplier);
            scanner.localPosition = new Vector2(-maxDistance / 2, 0);
        }

        //Bomb
        if (Input.GetKey(KeyCode.Space) && IsWorking && CurBombSpawnDelay <= 0)
        {
            Instantiate(bombPrefab, transform.position, transform.rotation);
            CurBombSpawnDelay = bombSpawnDelay;
        }

        if (CurBombSpawnDelay > 0) CurBombSpawnDelay -= Time.deltaTime * bombReloadSpeedMultiplier;
    }
}
