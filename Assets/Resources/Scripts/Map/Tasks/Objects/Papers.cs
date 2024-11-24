using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Papers : UsableObject
{
    [SerializeField] private AudioSource sound;

    [SerializeField] private Backlit3D backlit;

    [SerializeField] private GameObject firstModel;
    [SerializeField] private GameObject secondModel;

    [SerializeField] private BoxCollider2D trigger;
    [SerializeField] private BoxCollider2D notTrigger;

    [SerializeField] private float triggerSetMultiplier = 1.1f;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float textAppearSpeed;

    [SerializeField] private GameObject particles;

    public static Action OnTake { get; set; }

    private bool hasTaken = false;

    private bool changeMaterialOnce = false;

    private void Update()
    {
        if (hasTaken)
        {
            text.color = new(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0, Time.deltaTime * textAppearSpeed));
            firstModel.SetActive(false);
            secondModel.SetActive(true);
            trigger.size = notTrigger.size * triggerSetMultiplier;
            trigger.offset = notTrigger.offset;
            
            if (!changeMaterialOnce)
            {
                foreach (Material m in backlit.Materials)
                {
                    m.SetFloat("_Smoothness", 0);
                }
                changeMaterialOnce = true;

                backlit.enabled = false;
            }

            return;
        }
        if (canBeTaked)
        {
            text.color = new(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 1, Time.deltaTime * textAppearSpeed));
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnTake?.Invoke();
                hasTaken = true;
                if (Config.Particles) Instantiate(particles, transform.position, Quaternion.identity);

                sound.volume = Config.Sound;
                sound.Play();
            }
        }
        else text.color = new(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0, Time.deltaTime * textAppearSpeed));

        firstModel.SetActive(true);
        secondModel.SetActive(false);
    }
}
