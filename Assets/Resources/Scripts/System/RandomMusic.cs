using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    [SerializeField] private AudioSource[] music;
    [SerializeField] private float volumeMultiplier;

    private void Awake()
    {
        foreach (AudioSource source in music)
        {
            source.playOnAwake = false;
        }

        int rand = Random.Range(0, music.Length);

        music[rand].Play();
    }

    private void Update()
    {
        foreach (AudioSource source in music)
        {
            source.volume = Config.Sound * volumeMultiplier;
        }
    }
}
