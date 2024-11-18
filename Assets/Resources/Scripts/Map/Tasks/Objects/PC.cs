using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PC : UsableObject
{
    public static bool HasUsed { get; private set; } = false;
    public static int CodeLength { get; set; }

    private bool isOpen = false;

    [SerializeField] private Image fade;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float textChangeSpeed;

    [SerializeField] private Volume volumePC;
    [SerializeField] private float volumeWeightSet;

    [SerializeField] private float volumeChangeSpeed;


    [Header("Main")]
    [SerializeField] private float fadeChangeSpeed;
    [SerializeField] private Transform[] canvas;
    [SerializeField] private float canvasZPosition;
    [SerializeField] private Material background;
    [SerializeField] private TextMeshProUGUI[] enterText;
    [SerializeField] private TMP_InputField inputField;


    [Header("Visuals")]
    [SerializeField] private float backgroundAppearSpeed;
    [SerializeField] private float backgroundDisappearSpeed;
    [SerializeField] private float backgroundColoringSpeed;

    [SerializeField] private float shakePower;

    private Volume defaultVolume;

    private Color startTextColor;
    private Color startBackgroundColor;

    private float time = 0f;
    private float backgroundSetTime = 0f;

    private void Start()
    {
        defaultVolume = CameraSetup.Instance.GetComponentInChildren<Volume>();

        volumePC.weight = 0;

        startTextColor = text.color;
        startBackgroundColor = background.color;

        Volume v = CameraSetup.Instance.gameObject.AddComponent<Volume>();
        v.profile = volumePC.profile;

        volumePC.enabled = false;
        volumePC = v;

        PostProcessingController.Instance.IsVolumeChanging = true;
    }

    private void OnDisable()
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
    }

    private void Update()
    {
        if (player != null)
        {
            foreach (Transform t in canvas)
            {
                t.position = new Vector3(player.transform.position.x, player.transform.position.y, canvasZPosition);
            }
        }

        foreach(TextMeshProUGUI _text in enterText)
        {
            _text.color = new Color(_text.color.a, _text.color.g, _text.color.b, background.color.a);
        }

        if (Input.GetKeyDown(KeyCode.E) && !HasUsed && canBeTaked | isOpen)
        {
            isOpen = !isOpen;
            time = 0f;
            backgroundSetTime = 0f;

            Controller.CanMove = !isOpen;
        }

        if (HasUsed || PlayerManager.Instance.HP <= -0.01f)
        {
            isOpen = false;
        }

        if (time < 1f) time += Time.deltaTime;

        if (canBeTaked && !HasUsed) text.color = Color.Lerp(text.color, startTextColor, Time.deltaTime * textChangeSpeed);
        else text.color = Color.Lerp(text.color, new(0, 0, 0, 0), Time.deltaTime * textChangeSpeed);

        background.color = Color.Lerp(background.color, new Color(startBackgroundColor.r, startBackgroundColor.g, startBackgroundColor.b, background.color.a), Time.deltaTime * backgroundColoringSpeed);

        if (isOpen)
        {
            inputField.characterLimit = CodeLength;

            fade.color = new Color(0, 0, 0, ProjMath.EaseInCubic(time));

            volumePC.weight = Mathf.Lerp(volumePC.weight, volumeWeightSet, Time.deltaTime * volumeChangeSpeed);
            defaultVolume.weight = Mathf.Lerp(defaultVolume.weight, 0, Time.deltaTime * volumeChangeSpeed);

            if (fade.color.a > 0.95f)
            {
                inputField.gameObject.SetActive(true);

                if (backgroundSetTime < 1f) backgroundSetTime += Time.deltaTime * backgroundAppearSpeed;
                background.color = new Color(background.color.r, background.color.g, background.color.b, ProjMath.EaseInOutBounce(backgroundSetTime));

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (PCTask.CheckCode(inputField.text))
                    {
                        background.color = Color.green;
                        HasUsed = true;
                        Controller.CanMove = true;
                    }

                    else
                    {
                        background.color = Color.red;
                        CameraShakeManager.Instance.Shake(PlayerManager.Instance.GetPlayerPosition().GetComponentInChildren<CinemachineImpulseSource>(), shakePower);
                    }
                }
            }
        }

        else
        {
            inputField.gameObject.SetActive(false);

            fade.color = new Color(0, 0, 0, Mathf.Lerp(fade.color.a, 0, Time.deltaTime * fadeChangeSpeed));

            volumePC.weight = Mathf.Lerp(volumePC.weight, 0, Time.deltaTime * volumeChangeSpeed);
            defaultVolume.weight = Mathf.Lerp(defaultVolume.weight, Config.PostProcessingPower, Time.deltaTime * volumeChangeSpeed);
            background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.Lerp(background.color.a, 0, Time.deltaTime * backgroundDisappearSpeed));
        }

        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, Mathf.Clamp(fade.color.a, 0f, 1f));
    }
}
