using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicscript : MonoBehaviour
{
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public AudioClip four;
    public AudioClip five;

    public int x = 0;
    bool delaying = false;

    AudioSource audiosource;

    void Awake()
    {
        if (FindObjectsOfType<musicscript>().Length != 1)
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

        audiosource.clip = one;
        StartCoroutine(SongDelay(120));

        x++;
    }

    private void Update()
    {
        if (!audiosource.isPlaying && !delaying)
        {
            switch (x)
            {
                case 1:
                    audiosource.clip = two;
                    StartCoroutine(SongDelay(600));
                    x++;
                    break;
                case 2:
                    audiosource.clip = three;
                    StartCoroutine(SongDelay(600));
                    x++;
                    break;
                case 3:
                    audiosource.clip = four;
                    StartCoroutine(SongDelay(600));
                    x++;
                    break;
                case 4:
                    audiosource.clip = five;
                    StartCoroutine(SongDelay(600));
                    x++;
                    break;
                case 5:
                    audiosource.clip = two;
                    StartCoroutine(SongDelay(600));
                    x = 1;
                    break;
            }
        }
    }

    IEnumerator SongDelay(float length)
    {
        delaying = true;

        int z = 0;

        while (z < length)
        {
            z++;
            yield return new WaitForEndOfFrame();
        }

        audiosource.Play();

        delaying = false;
    }
}
