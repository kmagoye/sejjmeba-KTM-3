using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitionscript : MonoBehaviour
{
    Color x;
    public float length;
    Rigidbody2D rb2d;
    public bool startup = true;

    void Awake()
    {
        if (FindObjectsOfType<transitionscript>().Length != 1)
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
        x = GetComponent<SpriteRenderer>().color;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = FindObjectOfType<Camera>().transform.position;
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = x;
    }

    public void FadeIn(int initialDelay)
    {
        StartCoroutine(fadeIn(initialDelay));
        if (startup)
        {
            startup = false;
        }
        else
        {
            FindObjectOfType<menuplayerscript>().MenutoMap(2f);
        }
    }
    IEnumerator fadeIn(int initialDelay)
    {
        x.a = 1;
        int z = 0;

        while(z < initialDelay)
        {
            z++;
            yield return new WaitForEndOfFrame();
        }

        z = 1;

        while (z < length)
        {
            x.a = x.a - 1f / length;
            z++;
            yield return new WaitForEndOfFrame();
        }

        x.a = 0;
    }

    public void FadeOut()
    {
         StartCoroutine(fadeOut());
    }
    IEnumerator fadeOut()
    {
        x.a = 0;

        int z = 1;

        while (z < length)
        {
            x.a = x.a + 1f / length;
            z++;
            yield return new WaitForEndOfFrame();
        }

        x.a = 1;
    }
}
