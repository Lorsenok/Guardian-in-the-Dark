using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    public static PostProcessingController Instance;
    public bool IsVolumeChanging { get; set; } = false; 

    [SerializeField] private float changeToNormalSpeed;

    private Volume volume;

    private bool isBloomChanges = false;
    private Bloom bloom;
    private float startBloomIntensity;

    private bool isVignetteChanges = false;
    private Vignette vignette;
    private float startVignetteIntensity;
    private Color startVignetteColor;

    private bool isLensDistortionChanges = false;
    private LensDistortion lensDistortion;
    private float startLensDistortionIntensity;

    private bool isChromaticAberrationChanges = false;
    private ChromaticAberration chromaticAberration;
    private float startChromaticAberrationIntensity;

    private void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();

        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);

        startBloomIntensity = bloom.intensity.value;

        startVignetteIntensity = vignette.intensity.value;
        startVignetteColor = vignette.color.value;
        
        startLensDistortionIntensity = lensDistortion.intensity.value;
        
        startChromaticAberrationIntensity = chromaticAberration.intensity.value;
    }

    public void BloomSet(float intensity, float speed)
    {
        isBloomChanges = true;

        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, intensity, Time.deltaTime * speed);
    }

    public void VignetteSet(float intensity, Color color, float speed)
    {
        isVignetteChanges = true;

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, intensity, Time.deltaTime * speed);
        vignette.color.value = Color.Lerp(vignette.color.value, color, Time.deltaTime * speed);
    }
    
    public void LensDistortionSet(float intensity, float speed)
    {
        isLensDistortionChanges = true;

        lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, intensity, Time.deltaTime * speed);
    }

    public void ChromaticAberrationSet(float intensity, float speed)
    {
        isChromaticAberrationChanges = true;

        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, intensity, Time.deltaTime * speed);
    }

    private void LateUpdate()
    {
        if (!isBloomChanges)
        {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, startBloomIntensity, Time.deltaTime * changeToNormalSpeed);
        }

        if (!isVignetteChanges)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, startVignetteIntensity, Time.deltaTime * changeToNormalSpeed);
            vignette.color.value = Color.Lerp(vignette.color.value, startVignetteColor, Time.deltaTime * changeToNormalSpeed);
        }

        if (!isLensDistortionChanges)
        {
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, startLensDistortionIntensity, Time.deltaTime * changeToNormalSpeed);
        }

        if (!isChromaticAberrationChanges)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, startChromaticAberrationIntensity, Time.deltaTime * changeToNormalSpeed);
        }

        isBloomChanges = false;
        isChromaticAberrationChanges = false;
        isLensDistortionChanges = false;
        isVignetteChanges = false;
    }
}
