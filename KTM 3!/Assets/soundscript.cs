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

    AudioSource audiosource;
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
        audiosource = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        if(sound == "move")
        {
            audiosource.volume = 0.8f;
            audiosource.clip = move;
            audiosource.Play();
        }
        if (sound == "undo")
        {
            audiosource.volume = 0.4f;
            audiosource.clip = undo;
            audiosource.Play();
        }
        if (sound == "swing")
        {
            audiosource.volume = 0.8f;
            audiosource.clip = swing;
            audiosource.Play();
        }
        if (sound == "menu")
        {
            audiosource.volume = 0.7f;
            audiosource.clip = menu;
            audiosource.Play();
        }
        if (sound == "bonk")
        {
            audiosource.volume = 0.5f;
            audiosource.clip = bonk;
            audiosource.Play();
        }
        if (sound == "goal")
        {
            audiosource.volume = 0.6f;
            audiosource.clip = goal;
            audiosource.Play();
        }
        if (sound == "error")
        {
            audiosource.volume = 0.3f;
            audiosource.clip = error;
            audiosource.Play();
        }
    }
}
