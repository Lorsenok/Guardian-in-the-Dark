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

    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform pointDirection;

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

    private float delay = 0;

    private float currectDifference;
    private float currectDifferenceSet;
    private float differenceDistance;

    private void OnDisable()
    {
        scanner.gameObject.SetActive(false);
    }

    private void Start()
    {
        differenceDistance = maxDifference - minDifference;
        currectDifference = minDifference + differenceDistance;
        curPointColor = startColorList[Random.Range(0, startColorList.Count-1)];
        currectDifferenceSet = minDifference + differenceDistance / 2;
        currectDifference = currectDifferenceSet;
    }

    Color curPointColor;
    private void SpawnRay()
    {
        Vector3 direction = -(transform.position - pointDirection.position);
        direction += new Vector3(Random.Range(-currectDifference, currectDifference), Random.Range(-currectDifference, currectDifference), 0);
        Vector3 pos = Physics2D.Raycast(pointDirection.position, direction.normalized, Mathf.Infinity, collideLayer).point;

        if (pos == Vector3.zero | Vector2.Distance(pos, transform.position) > maxDistance) return;


        //Spawn point and ray
        GameObject point = Instantiate(pointPrefab, pos, Quaternion.identity);
        point.GetComponent<SpriteRenderer>().color = curPointColor;
        
        int rgbChoose = Random.Range(1, 3);

        if (rgbChoose == 1) curPointColor.r += colorChangeSpeed;
        if (rgbChoose == 2) curPointColor.g += colorChangeSpeed;
        if (rgbChoose == 3) curPointColor.b += colorChangeSpeed;

        if (curPointColor.r > colorRGBMax) curPointColor.r = colorRGBMin;
        if (curPointColor.g > colorRGBMax) curPointColor.g = colorRGBMin;
        if (curPointColor.b > colorRGBMax) curPointColor.b = colorRGBMin;

        Vector3 pointPos = point.transform.position;
        
        Transform rayToPointStart = Instantiate(pointRay, pointDirection.position, Quaternion.identity).transform;
        Transform rayToPoint = rayToPointStart.GetComponentInChildren<DisolvingObject>().transform;


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

        //Shooting
        if (delay <= 0 & Input.GetMouseButton(0))
        {
            delay = speedS + currectDifference * speedDecreaser;
            SpawnRay();
        }
        if (delay > 0) delay -= Time.deltaTime;


        //Scroll
        //currectDifferenceSet += -Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetAxis("Mouse ScrollWheel") > 0) currectDifferenceSet -= differenceDistance / divisionsQuantity;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) currectDifferenceSet += differenceDistance / divisionsQuantity;
        currectDifferenceSet = Mathf.Clamp(currectDifferenceSet, minDifference, maxDifference);

        currectDifference = Mathf.Lerp(currectDifference, currectDifferenceSet, Time.deltaTime * differenceChangeSpeed);

        scanner.gameObject.SetActive(Input.GetMouseButton(0));

        //Scanner
        if (Input.GetMouseButton(0))
        {
            scanner.localScale = new Vector2(maxDistance, currectDifference * scannerScaleYMultiplier);
            scanner.localPosition = new Vector2(-maxDistance/2, 0);
        }
    }
}
