using Cinemachine;
using Pathfinding;
using Pathfinding.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IDamageable
{
    void GetDamage(int damage);
    void Die();
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

    [SerializeField] protected Material[] materials;

    [SerializeField] private float additionalAngle;

    [SerializeField] private float shakePower;

    public static Action OnEnemyDestroyed;

    protected bool isDead = false;

    protected Rigidbody2D rg;
    protected AIPath follow;
    protected AIDestinationSetter destinationSetter;
    protected Backlit3D backlit;

    protected Transform player;

    private void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player && PlayerManager.Instance.HP > 0)
        {
            PlayerManager.Instance.HP = 0;
            CameraShakeManager.instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), shakePower);
        }
    }

    public virtual void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed = 0;
    }

    public virtual void Die()
    {
        isDead = true;
        timeBeforeDeathStart = timeBeforeDeath;
    }

    protected void RotateTowardsPlayer()
    {
        Vector3 _diference = player.position - transform.position;
        float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ + additionalAngle);
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

        if (player != null) RotateTowardsPlayer();
    }

    protected float timeBeforeDeathStart;
    public virtual void Update()
    {
        if (isDead)
        {
            GetComponent<AIPath>().enabled = false;

            if (backlit.enabled)
            {
                backlit.enabled = false;
            }
            else
            {
                backlit.Lighting.intensity = Mathf.Lerp(backlit.Lighting.intensity, 0, Time.deltaTime * 10);
            }

            if (timeBeforeDeath <= 0)
            {
                foreach (Material mat in materials) 
                    mat.SetFloat("_Disolve", 0);
                Destroy(gameObject);
            }
            foreach (Material mat in materials)
            {
                mat.SetFloat("_Disolve", 1.1f - 1.1f / timeBeforeDeathStart * timeBeforeDeath);
                mat.SetFloat("_Smoothness", 0.5f - 0.5f / timeBeforeDeathStart * timeBeforeDeath);
            }
            timeBeforeDeath -= Time.deltaTime;

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
