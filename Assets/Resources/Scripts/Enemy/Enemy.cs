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
    [SerializeField] protected float hp;
    [SerializeField] protected float speed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float timeBeforeAgr;
    [SerializeField] protected float timeBeforeDeath;

    public static Action OnEnemyDestroyed;

    protected bool isDead = false;

    protected Rigidbody2D rg;
    protected Material material;
    protected AIPath follow;
    protected AIDestinationSetter destinationSetter;
    protected Backlit backlit;

    protected Transform player;

    private void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            PlayerManager.Instance.HP = 0;
        }
    }

    public virtual void GetDamage(int damage)
    {
        hp -= damage;
    }

    public virtual void Die()
    {
        isDead = true;
        timeBeforeDeathStart = timeBeforeDeath;
    }

    public virtual void Follow(Transform player)
    {
        /*
        Vector3 _diference = player.position - transform.position;
        float _rotateZ = Mathf.Atan2(_diference.y, _diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);

        rg.position = Vector2.MoveTowards(rg.position, player.position, speed * Time.deltaTime);
        */

        if (!follow.enabled)
        {
            follow.enabled = true;
            destinationSetter.enabled = true;
            destinationSetter.target = player;
            follow.maxSpeed = speed;
            follow.maxAcceleration = acceleration;
        }
    }

    public virtual void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        material = GetComponent<SpriteRenderer>().material;
        follow = GetComponent<AIPath>();
        follow.enabled = false;
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.enabled = false;
        backlit = GetComponent<Backlit>();
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
                backlit.Lighting.intensity = Mathf.Lerp(backlit.Lighting.intensity, 1, Time.deltaTime * 10);
            }

            if (timeBeforeDeath <= 0)
            {
                material.SetFloat("_Disolve", 0);
                Destroy(gameObject);
            }
            material.SetFloat("_Disolve", 1.1f - 1.1f / timeBeforeDeathStart * timeBeforeDeath);
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
