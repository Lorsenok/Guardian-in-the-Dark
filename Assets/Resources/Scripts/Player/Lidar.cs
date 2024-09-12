using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    [Header("Lidar setup")]
    [SerializeField] private float speedS;
    [SerializeField] private float speedDecreaser;
    [SerializeField] private float minDifference;
    [SerializeField] private float maxDifference;
    [SerializeField] private float maxDistance;
    [SerializeField] private float divisionsQuantity;
    [SerializeField] private float differenceChangeSpeed;
    [SerializeField] private float differenceMultiplier;

    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform pointDirection;

    [SerializeField] private float minDistance;

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

    public bool IsWorking { get; set; } = true;
    public float TimeSinceLastRay { get; set; } = 0f;

    private SpriteRenderer scannerSpr;

    private float delay = 0;

    private float currectDifference;
    private float currectDifferenceSet;
    private float differenceDistance;

    private void Start()
    {
        scannerSpr = scanner.GetComponentInChildren<SpriteRenderer>();

        differenceDistance = maxDifference - minDifference;
        currectDifference = minDifference + differenceDistance;
        curPointColor = startColorList[Random.Range(0, startColorList.Count)];
        currectDifferenceSet = minDifference + differenceDistance / 2;
        currectDifference = currectDifferenceSet;
    }

    public RaycastHit2D CurrectRay { get; set; }

    Color curPointColor;
    private void SpawnRay()
    {
        TimeSinceLastRay = 0f;

        Vector3 direction = -(transform.position - pointDirection.position);
        float diff = currectDifference * differenceMultiplier;
        direction += new Vector3(Random.Range(-diff, diff), Random.Range(-diff, diff), 0);
        RaycastHit2D raycast = Physics2D.Raycast(pointDirection.position, direction.normalized, Mathf.Infinity, collideLayer);

        CurrectRay = raycast;

        Vector3 pos = raycast.point;

        if (pos == Vector3.zero | Vector2.Distance(pos, transform.position) > maxDistance) return;


        //Spawn point and ray
        GameObject point = Instantiate(pointPrefab, pos, Quaternion.identity);
        point.GetComponent<SpriteRenderer>().color = curPointColor;

        if (Vector2.Distance(point.transform.position, pointDirection.position) < minDistance)
        {
            Destroy(point);
            return;
        }
        
        if (curPointColor != Color.white)
        {
            int rgbChoose = Random.Range(1, 3);

            if (rgbChoose == 1) curPointColor.r += colorChangeSpeed;
            if (rgbChoose == 2) curPointColor.g += colorChangeSpeed;
            if (rgbChoose == 3) curPointColor.b += colorChangeSpeed;

            if (curPointColor.r > colorRGBMax) curPointColor.r = colorRGBMin;
            if (curPointColor.g > colorRGBMax) curPointColor.g = colorRGBMin;
            if (curPointColor.b > colorRGBMax) curPointColor.b = colorRGBMin;
        }

        raycast.collider.gameObject.TryGetComponent(out Backlit backlit);
        raycast.collider.gameObject.TryGetComponent(out Backlit3D backlit3d);

        if (backlit != null)
        {
            backlit.Light();
            point.GetComponent<SpriteRenderer>().color = backlit.color;
        }
        if (backlit3d != null)
        {
            backlit3d.Light();
            point.GetComponent<SpriteRenderer>().color = backlit3d.color;
        }

        Vector3 pointPos = point.transform.position;
        
        Transform rayToPointStart = Instantiate(pointRay, pointDirection.position, Quaternion.identity).transform;
        Transform rayToPoint = rayToPointStart.GetComponentInChildren<SpriteRenderer>().transform;


        //Rotate ray to point
        Vector3 diference = pointPos - rayToPointStart.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        rayToPointStart.transform.rotation = Quaternion.Euler(0, 0, rotateZ);


        //Ray transform setup
        float distance = Vector2.Distance(rayToPointStart.position, pointPos);

        rayToPoint.localScale = new Vector2(distance, rayWidth);
        rayToPoint.localPosition = new Vector2(distance / 2, 0);
    }

    private void Update()
    {
        if (!Controller.CanMove) return;

        TimeSinceLastRay += Time.deltaTime;

        //Shooting
        if (delay <= 0 & Input.GetMouseButton(0) && IsWorking && PlayerManager.Instance.IsMenuClosed)
        {
            delay = speedS + currectDifference * speedDecreaser;
            SpawnRay();
        }
        if (delay > 0) delay -= Time.deltaTime;


        //Scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0) currectDifferenceSet -= differenceDistance / divisionsQuantity;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) currectDifferenceSet += differenceDistance / divisionsQuantity;
        currectDifferenceSet = Mathf.Clamp(currectDifferenceSet, minDifference, maxDifference);

        currectDifference = Mathf.Lerp(currectDifference, currectDifferenceSet, Time.deltaTime * differenceChangeSpeed);

        if (Input.GetMouseButton(0) && IsWorking && PlayerManager.Instance.IsMenuClosed)
            scannerSpr.color = new Color(scannerSpr.color.r, scannerSpr.color.g, scannerSpr.color.b, Mathf.Lerp(scannerSpr.color.a, 1, Time.deltaTime * scannerAppearSpeed));
        else 
            scannerSpr.color = new Color(scannerSpr.color.r, scannerSpr.color.g, scannerSpr.color.b, Mathf.Lerp(scannerSpr.color.a, 0, Time.deltaTime * scannerAppearSpeed));

        //Scanner
        if (Input.GetMouseButton(0))
        {
            scanner.localScale = new Vector2(maxDistance, currectDifference * scannerScaleYMultiplier);
            scanner.localPosition = new Vector2(-maxDistance/2, 0);
        }
    }
}
