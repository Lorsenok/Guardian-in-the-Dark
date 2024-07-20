using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnemy : Enemy
{
    [Header("Worm")]
    [SerializeField] private float force;
    [SerializeField] private float impulseDelay;
    [SerializeField] private float followEnabledDelay;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float collideShakePower;
    [SerializeField] private float forceSlowDownSpeed;

    [Header("Worm Sigments")]
    [SerializeField] private GameObject[] sigments;
    [SerializeField] private float sigmentsSpawnDistance;
    [SerializeField] private float additionalSigmentsAngle;
    private int curSigment = 0;
    private GameObject curSigmentObj;
    private Vector3 startPosition;

    private float curImpulseDelay;
    private float curFollowEnabledDelay;

    public override void Start()
    {
        startPosition = transform.position;
        curSigmentObj = gameObject;
        curImpulseDelay = impulseDelay;
        base.Start();
    }

    public override void Follow(Transform player)
    {
        base.Follow(player);
        follow.enabled = curFollowEnabledDelay <= 0;
    }

    Quaternion startRotation;
    Quaternion endRotation;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer != gameObject.layer)
        {
            CameraShakeManager.instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), collideShakePower);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            curFollowEnabledDelay *= forceSlowDownSpeed * Time.deltaTime;
        }
    }

    public override void Update()
    {
        if (follow.enabled != (curFollowEnabledDelay <= 0)) follow.enabled = curFollowEnabledDelay <= 0;

        base.Update();

        if (Vector2.Distance(curSigmentObj.transform.position, startPosition) > sigmentsSpawnDistance && curSigment < sigments.Length)
        {
            Transform obj = Instantiate(sigments[curSigment], startPosition, Quaternion.identity).transform;

            Vector3 _diference = curSigmentObj.transform.position - obj.position;
            float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
            obj.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _rotateZ + additionalSigmentsAngle), speed * Time.deltaTime);

            obj.GetComponent<WormSection>().Enemy = this;
            obj.GetComponent<HingeJoint2D>().connectedBody = curSigmentObj.GetComponent<Rigidbody2D>();

            curSigmentObj = obj.gameObject;
            curSigment += 1;
        }

        if (PlayerManager.Instance.HP <= 0)
        {
            follow.enabled = true;
            return;
        }

        if (curFollowEnabledDelay <= 0) curImpulseDelay -= Time.deltaTime;
        if (curImpulseDelay <= 0)
        {
            startRotation = transform.rotation;
            RotateTowardsPosition(player.position);
            endRotation = transform.rotation;
            transform.rotation = startRotation;

            curImpulseDelay = impulseDelay;
            curFollowEnabledDelay = followEnabledDelay;
            rg.AddForce((player.position - transform.position).normalized * force, ForceMode2D.Impulse);
        }

        if (curFollowEnabledDelay > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, rotateSpeed * Time.deltaTime);
        }

        curFollowEnabledDelay -= Time.deltaTime;
    }
}
