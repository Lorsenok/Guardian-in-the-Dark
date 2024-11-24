using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource[] music;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool loop = true;

    private bool playing = false;

    private AudioSource curPlaying;

    public void Play()
    {
        curPlaying = music[0];
        curPlaying.enabled = true;
        curPlaying.Play();

        playing = true;

        foreach (AudioSource m in music)
        {
            if (m != curPlaying) m.enabled = false;
        }
    }

    private void Awake()
    {
        foreach (AudioSource m in music)
        {
            m.playOnAwake = false;
        }

        if (playOnAwake)
        {
            Play();
        }
    }

    private void Update()
    {
        foreach (AudioSource m in music)
        {
            m.volume = Config.Music;
        }

        if (!playing)
        {
            foreach (AudioSource m in music)
            {
                m.enabled = false;
            }

            return;
        }

        if (!curPlaying.isPlaying)
        {
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i] == curPlaying)
                {
                    if (i == music.Length - 1)
                    {
                        playing = loop;
                        if (loop) Play();
                    }
                    else
                    {
                        music[i].enabled = false;

                        curPlaying = music[i + 1];
                        music[i + 1].enabled = true;
                        
                        curPlaying.Play();

                    }

                    break;
                }
            }
        }
    }
}
