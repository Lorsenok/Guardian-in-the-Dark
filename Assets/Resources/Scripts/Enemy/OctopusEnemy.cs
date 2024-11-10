using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OctopusEnemy : Enemy 
{
    [Header("Octopus")]
    [SerializeField] private GameObject octopusPartPrefab;
    [SerializeField] private Transform[] partsPositions;

    [SerializeField] private GameObject fadePrefab;
    [SerializeField] private float fadeAppearSpeed;
    [SerializeField] private float fadeSpeed = 1f;

    private Image fade;
    private Transform fadeT;

    private bool wingsDisabled = false;

    public override void Start()
    {
        foreach (Transform part in partsPositions)
        {
            Instantiate(octopusPartPrefab, part.transform.position, part.transform.rotation);
        }

        fadeT = Instantiate(fadePrefab, transform.position, Quaternion.identity).transform;
        fade = fadeT.gameObject.GetComponentInChildren<Image>();

        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (hp <= 0) fade.color = Color.Lerp(fade.color, new Color(fade.color.r, fade.color.g, fade.color.b, 0), Time.deltaTime * fadeAppearSpeed);
        else fade.color = Color.Lerp(fade.color, new Color(fade.color.r, fade.color.g, fade.color.b, ProjMath.SinTime(m:fadeSpeed)), fadeAppearSpeed);

        fadeT.transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    public override void Die(bool fromWeapon = true)
    {
        if (!wingsDisabled)
        {
            foreach (WingedEnemyWing w in WingedEnemyWing.WingedEnemyWings)
            {
                if (w.GetComponent<Collider2D>())
                {
                    w.GetComponent<BoxCollider2D>().enabled = false;
                }
            }

            wingsDisabled = true;
        }
        
        base.Die(fromWeapon);
    }
}
