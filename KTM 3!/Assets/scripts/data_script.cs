using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_script : MonoBehaviour
{
    public List<int> times = new List<int>();
    public List<string> lastmoves = new List<string>();

    float InitialTime;

    void Awake()
    {
        DontDestroyOnLoad(this);
        InitialTime = Time.realtimeSinceStartup;
    }

    public void EndLevel()
    {
        lastmoves.Add("||");
        times.Add(Mathf.RoundToInt(Time.realtimeSinceStartup - InitialTime));
    }

    public void SetTime()
    {
        InitialTime = Time.realtimeSinceStartup;
    }

    public void LastMove(int x, string name)
    {
        if (x == 1)
        {
            lastmoves.Add("up" + "," + name + " ");
        }
        if (x == 2)
        {
            lastmoves.Add("down" + "," + name + " ");
        }
        if (x == 3)
        {
            lastmoves.Add("left" + "," + name + " ");
        }
        if (x == 4)
        {
            lastmoves.Add("right" + "," + name + " ");
        }
        if (x == 5)
        {
            lastmoves.Add("space" + "," + name + " ");
        }
    }
}
