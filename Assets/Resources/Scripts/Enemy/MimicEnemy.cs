using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MimicEnemy : Enemy
{
    [Header("Mimic")]
    [SerializeField] private GameObject mainModel;
    [SerializeField] private GameObject mimicModel;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float transformationSpeed;
    private bool isHostile = false;

    public override void GetDamage(int damage)
    {
        hp -= damage;
        currectSpeed /= 2;
        if (damage > 1) currectSpeed = 0;
        isHostile = true;
    }

    public override void Update()
    {
        dialogue.transform.rotation = Quaternion.identity;
        dialogue.gameObject.SetActive(!isHostile);

        if (!Controller.CanMove) isHostile = true;

        if (isHostile)
        {
            mainModel.transform.localScale = Vector3.Lerp(mainModel.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * transformationSpeed);
            mimicModel.transform.localScale = Vector3.Lerp(mimicModel.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * transformationSpeed);
            Em.IsEnemyAlive = true;
            follow.enabled = true;
        }

        else
        {
            if (!dialogue.IsWorking)
            {
                isHostile = true;
            }
            else
            {
                Em.IsEnemyAlive = false;
                follow.enabled = false;
                SmoothRotateTowardsPosition(rotateSpeed, player.position);
                return;
            }
        }

        base.Update();
    }
}
