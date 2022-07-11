using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundscript : MonoBehaviour
{
    public AudioClip move;
    public AudioClip undo;
    public AudioClip swing;
    public AudioClip goal;
    public AudioClip bonk;
    public AudioClip menu;
    public AudioClip error;

    AudioSource audio;
    void Awake()
    {
        if (FindObjectsOfType<soundscript>().Length != 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        if(sound == "move")
        {
            audio.volume = 1f;
            audio.clip = move;
            audio.Play();
        }
        if (sound == "undo")
        {
            audio.volume = 0.4f;
            audio.clip = undo;
            audio.Play();
        }
        if (sound == "swing")
        {
            audio.volume = 1f;
            audio.clip = swing;
            audio.Play();
        }
        if (sound == "menu")
        {
            audio.volume = 0.7f;
            audio.clip = menu;
            audio.Play();
        }
        if (sound == "bonk")
        {
            audio.volume = 0.5f;
            audio.clip = bonk;
            audio.Play();
        }
        if (sound == "goal")
        {
            audio.volume = 0.7f;
            audio.clip = goal;
            audio.Play();
        }
        if (sound == "error")
        {
            audio.volume = 0.3f;
            audio.clip = error;
            audio.Play();
        }
    }
}
