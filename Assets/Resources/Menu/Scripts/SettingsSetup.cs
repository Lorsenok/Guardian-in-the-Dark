using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSetup : MonoBehaviour
{
    public static SettingsSetup Instance { get; private set; }

    [SerializeField] private Menu menu;

    [SerializeField] private Slider sound;
    [SerializeField] private SettingsButton pointSounds;
    [SerializeField] private Slider music;

    [SerializeField] private SettingsButton cameraShake;
    [SerializeField] private SettingsButton particles;

    [SerializeField] private Slider power;

    [SerializeField] private SettingsButton analogGlitchEffect;
    [SerializeField] private SettingsButton fisheyeEffect;

    [SerializeField] private TMP_InputField pixelizationPower;

    public void ClearSettings()
    {
        PlayerPrefs.SetFloat("sound", Config.SoundDefault);
        PlayerPrefs.SetInt("points", Config.LidarPointSoundsDefault ? 1 : 0);
        PlayerPrefs.SetFloat("music", Config.MusicDefault);

        PlayerPrefs.SetInt("shake", Config.CameraShakeDefault ? 1 : 0);
        PlayerPrefs.SetInt("particles", Config.ParticlesDefault ? 1 : 0);

        PlayerPrefs.SetFloat("power", Config.PostProcessingPowerDefault);

        PlayerPrefs.SetInt("analog", Config.AnalogGlitchEffectDefault ? 1 : 0);
        PlayerPrefs.SetInt("fisheye", Config.FisheyeEffectDefault ? 1 : 0);

        PlayerPrefs.SetInt("pixelization", Config.PixelizationDefault);

        Load();
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            sound.value = PlayerPrefs.GetFloat("sound");
            pointSounds.IsOn = PlayerPrefs.GetInt("points") == 1;
            music.value = PlayerPrefs.GetFloat("music");

            cameraShake.IsOn = PlayerPrefs.GetInt("shake") == 1;
            particles.IsOn = PlayerPrefs.GetInt("particles") == 1;

            power.value = PlayerPrefs.GetFloat("power");

            analogGlitchEffect.IsOn = PlayerPrefs.GetInt("analog") == 1;
            fisheyeEffect.IsOn = PlayerPrefs.GetInt("fisheye") == 1;

            pixelizationPower.text = PlayerPrefs.GetInt("pixelization").ToString();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        Config.Sound = sound.value;
        Config.LidarPointSounds = pointSounds.IsOn;
        Config.Music = music.value;

        Config.CameraShake = cameraShake.IsOn;
        Config.Particles = particles.IsOn;

        Config.PostProcessingPower = power.value;

        Config.AnalogGlitchEffect = analogGlitchEffect.IsOn;
        Config.FisheyeEffect = fisheyeEffect.IsOn;

        if (pixelizationPower.text != string.Empty && pixelizationPower.text != "" && pixelizationPower.text != "-") Config.Pixelization = int.Parse(pixelizationPower.text);

        if (!menu.Open) return;

        PlayerPrefs.SetFloat("sound", sound.value);
        PlayerPrefs.SetInt("points", pointSounds.IsOn ? 1 : 0);
        PlayerPrefs.SetFloat("music", music.value);

        PlayerPrefs.SetInt("shake", cameraShake.IsOn ? 1 : 0);
        PlayerPrefs.SetInt("particles", particles.IsOn ? 1 : 0);

        PlayerPrefs.SetFloat("power", power.value);
        
        PlayerPrefs.SetInt("analog", analogGlitchEffect.IsOn ? 1 : 0);
        PlayerPrefs.SetInt("fisheye", fisheyeEffect.IsOn ? 1 : 0);

        if (pixelizationPower.text != string.Empty && pixelizationPower.text != "" && pixelizationPower.text != "-") PlayerPrefs.SetInt("pixelization", int.Parse(pixelizationPower.text));
    }
}
