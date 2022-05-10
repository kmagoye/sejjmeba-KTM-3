using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_script : MonoBehaviour
{
    public List<int> times = new List<int>();
    public List<string> lastmoves = new List<string>();
    int a;
    int b;
    int c;

    float InitialTime;

    void Awake()
    {
        DontDestroyOnLoad(this);
        InitialTime = Time.realtimeSinceStartup;
    }

    public void EndLevel()
    {
        print("asd");

        times.Add(Mathf.RoundToInt(Time.realtimeSinceStartup - InitialTime));
    }

    public void SetTime()
    {
        InitialTime = Time.realtimeSinceStartup;
    }

    public void PlayerInput(int x)
    {
        if (x == 1)
        {
            lastmoves.Add("up");
        }
        if (x == 2)
        {
            lastmoves.Add("down");
        }
        if (x == 3)
        {
            lastmoves.Add("left");
        }
        if (x == 4)
        {
            lastmoves.Add("right");
        }
        if (x == 5)
        {
            lastmoves.Add("space");
        }
    }
}
