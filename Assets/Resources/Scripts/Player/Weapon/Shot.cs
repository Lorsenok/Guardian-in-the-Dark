using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float Damage {  get; set; }
    public float TimeBeforeShot { get; set; } = 0;

    private bool shooted = false;

    [SerializeField] private float additionalSize;
    [SerializeField] private float width;

    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject preObj;

    [SerializeField] private Vector2 preObjSizeGrown;
    [SerializeField] private float preObjSizeGrownSpeed;

    [SerializeField] private bool shake;
    [SerializeField] private float shakePower;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage things against an enemy
    }

    private void Start()
    {
        Damage += Config.WeaponDamage;
        preObj.transform.localScale = new Vector3(0, 0, 1);
    }

    public void Work(Transform _direction, Transform _position, LayerMask _collideLayer)
    {
        shooted = true;
        position = _position;
        direction = _direction;
        collideLayer = _collideLayer;
    }

    private Transform direction;
    private Vector3 curDirection;
    private Transform position;
    private LayerMask collideLayer;

    private bool once = false;
    private void Update()
    {
        TimeBeforeShot -= Time.deltaTime;
        if (!shooted || TimeBeforeShot > 0)
        {
            if (!once)
            {
                transform.position = position.position;

                Vector3 _diference = direction.position - transform.position;
                float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, _rotateZ);

                curDirection = direction.position;

                preObj.transform.localScale = Vector3.Lerp(preObj.transform.localScale, preObjSizeGrown, preObjSizeGrownSpeed * Time.deltaTime);
                obj.transform.localScale = Vector3.zero;
            }
            return;
        }

        if (!once)
        {
            if (shake) CameraShakeManager.instance.Shake(GetComponent<CinemachineImpulseSource>(), shakePower);
            preObj.SetActive(false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            once = true;
        }

        float acc = Config.WeaponAccuracy;

        Vector3 _direction = -(transform.position - curDirection);
        _direction += new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), 0);
        Vector3 pos = Physics2D.Raycast(curDirection, _direction.normalized, Mathf.Infinity, collideLayer).point;

        Vector3 diference = pos - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);

        float distance = Vector2.Distance(pos, obj.transform.position);
        transform.localScale = new Vector2(distance + additionalSize, width);

        shooted = false;
    }
}
