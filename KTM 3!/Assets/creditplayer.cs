using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class creditplayer : MonoBehaviour
{
    public CinemachineVirtualCamera up;
    public CinemachineVirtualCamera down;
    public CinemachineVirtualCamera left;
    public CinemachineVirtualCamera right;
    public CinemachineVirtualCamera mid;

    int state = 0; //0 mid, 1 up, 2 right, 3 down, 4 left

    private void Start()
    {
        FindObjectOfType<transitionscript>().FadeIn(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            FindObjectOfType<transitionscript>().startup = true;
            FindObjectOfType<transitionscript>().FadeOut();
            StartCoroutine(Delay());
        }

        if (Input.GetKeyDown("up"))
        {
            if(state == 0)
            {
                up.Priority = 1;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 0;

                state = 1;
            }
            if(state == 3)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 1;

                state = 0;
            }
        }
        if (Input.GetKeyDown("down"))
        {
            if (state == 0)
            {
                up.Priority = 0;
                down.Priority = 1;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 0;

                state = 3;
            }
            if (state == 1)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 1;

                state = 0;
            }
        }
        if (Input.GetKeyDown("left"))
        {
            if (state == 0)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 1;
                right.Priority = 0;
                mid.Priority = 0;

                state = 4;
            }
            if (state == 2)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 1;

                state = 0;
            }
        }
        if (Input.GetKeyDown("right"))
        {
            if (state == 0)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 1;
                mid.Priority = 0;

                state = 2;
            }
            if (state == 4)
            {
                up.Priority = 0;
                down.Priority = 0;
                left.Priority = 0;
                right.Priority = 0;
                mid.Priority = 1;

                state = 0;
            }
        }
    }

    IEnumerator Delay()
    {
        float x = FindObjectOfType<transitionscript>().length;
        float y = 0;

        while (y < x)
        {
            y++;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("map");
    }
}
