using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RUNEnemy : Enemy
{
    [Header("RUN")]
    [SerializeField] private GameObject[] sprites;
    [SerializeField] private float spriteChangeDelaySet;
    [SerializeField] private float spriteChangeDelayBias;

    private float curSpriteChangeDelay = 0f;
    private int previousSpriteId = -1;

    public override void GetDamage(int damage)
    {
        return;
    }

    public override void Update()
    {
        curSpriteChangeDelay -= Time.deltaTime;

        if (curSpriteChangeDelay <= 0f)
        {
            curSpriteChangeDelay = spriteChangeDelaySet + Random.Range(-spriteChangeDelayBias, spriteChangeDelayBias);

            int rand = 0;

            for (int randi = -1; randi != previousSpriteId; randi = Random.Range(0, sprites.Length-1)) rand = randi;

            previousSpriteId = rand;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SetActive(i == rand);
            }
        }

        base.Update();
    }
}
