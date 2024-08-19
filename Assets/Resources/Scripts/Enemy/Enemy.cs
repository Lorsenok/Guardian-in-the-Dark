using Cinemachine;
using Pathfinding;
using Pathfinding.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IDamageable
{
    void GetDamage(int damage);
    void Die(bool fromWeapon = true);
}

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyManager em { get; set; }

    [SerializeField] protected float hp;
    [SerializeField] protected float speed;
    protected float currectSpeed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float speedAcceleration;
    [SerializeField] protected float timeBeforeAgr;
    [SerializeField] protected float timeBeforeDeath;

    [SerializeField] protected Light[] lights;
    [SerializeField] protected Material[] materials;

    [SerializeField] protected float additionalAngle;

    [SerializeField] protected float fallSpeed;

    public float ShakePower;

    public static Action OnEnemyDestroyed;

    protected bool isDead = false;
    private bool isDeadFromWeapon = true;

    protected Rigidbody2D rg;
    protected AIPath follow;
    protected AIDestinationSetter destinationSetter;
    protected Backlit3D backlit;

    protected Transform player;

    protected void Shake(float power)
    {
        CameraShakeManager.instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), power);
    }

    private void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            Shake(ShakePower);
        }
    }

    public virtual void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed = 0;
    }

    public virtual void Die(bool fromWeapon = true)
    {
        isDead = true;
        timeBeforeDeathStart = timeBeforeDeath;
        isDeadFromWeapon = fromWeapon;

        backlit.enabled = false;
        lightSet = GetComponentsInChildren<LightSet3D>();
        follow.enabled = false;

        if (backlit != null) startLightingIntensity = backlit.Lighting.intensity;

        foreach (LightSet3D light in lightSet)
        {
            light.enabled = false;
        }
    }

    protected void RotateTowardsPosition(Vector3 position)
    {
        Vector3 _diference = player.position - transform.position;
        float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ + additionalAngle);
    }

    protected void SmoothRotateTowardsPosition(float speed, Vector3 pos)
    {
        Vector3 _diference = pos - transform.position;
        float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _rotateZ + additionalAngle), speed * Time.deltaTime);
    }

    public virtual void Follow(Transform player)
    {
        currectSpeed = Mathf.Clamp(Mathf.Lerp(currectSpeed, speed, speedAcceleration * Time.deltaTime), 0.0f, speed);

        follow.enabled = true;
        destinationSetter.enabled = true;
        destinationSetter.target = player;
        follow.maxSpeed = currectSpeed;
        follow.maxAcceleration = acceleration;
    }

    public virtual void Start()
    {
        currectSpeed = speed;

        rg = GetComponent<Rigidbody2D>();
        follow = GetComponent<AIPath>();
        follow.enabled = false;
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.enabled = false;
        backlit = GetComponent<Backlit3D>();

        foreach (Material mat in materials)
        {
            mat.SetFloat("_Disolve", 0);
            mat.SetFloat("_Smoothness", 0.5f);
        }

        player = PlayerManager.Instance.GetPlayerPosition();

        if (player != null) RotateTowardsPosition(player.position);
    }

    protected float timeBeforeDeathStart;

    private LightSet3D[] lightSet;

    private float startLightingIntensity;

    public virtual void Update()
    {
        if (isDead)
        {
            backlit.Lighting.intensity = startLightingIntensity / timeBeforeDeathStart * timeBeforeDeath;

            if (timeBeforeDeath <= -0.35f)
            {
                foreach (Material mat in materials)
                    mat.SetFloat("_Disolve", 0);
                Destroy(gameObject);
            }
            foreach (Material mat in materials)
            {
                if (isDeadFromWeapon) mat.SetFloat("_Disolve", 1.1f - 1.1f / timeBeforeDeathStart * timeBeforeDeath);
                mat.SetFloat("_Smoothness", 0.5f / timeBeforeDeathStart * timeBeforeDeath);
            }

            if (lights != null)
            {
                foreach (Light l in lights)
                {
                    l.intensity = Mathf.Lerp(l.intensity, 0, Time.deltaTime);
                }
            }

            timeBeforeDeath -= Time.deltaTime;

            if (!isDeadFromWeapon)
            {
                Vector3 dir = (transform.position - player.position).normalized;
                rg.velocity += new Vector2(dir.x, dir.y) * Time.deltaTime;
            }

            transform.position += new Vector3(0, 0, fallSpeed * Time.deltaTime);

            return;
        }

        if (hp <= 0)
        {
            Die();
            return;
        }

        if (player == null & PlayerManager.Instance.GetPlayerPosition() == null) return;
        else if (player == null) player = PlayerManager.Instance.GetPlayerPosition();

        if (timeBeforeAgr > 0)
        {
            timeBeforeAgr -= Time.deltaTime;
            return;
        }

        Follow(player);
    }
}
