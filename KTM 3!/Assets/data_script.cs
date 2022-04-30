using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_script : MonoBehaviour
{
    public List<int> times = new List<int>();
    public List<int> moves = new List<int>();         //1, a
    public List<int> resets = new List<int>();         //2, b
    public List<int> missedinputs = new List<int>();   //3, c
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
        moves.Add(a);
        resets.Add(b);
        missedinputs.Add(c);
        a = b = c = 0;
    }

    public void SetTime()
    {
        InitialTime = Time.realtimeSinceStartup;
    }

    public void PlayerInput(int x)
    {
        if(x == 1)
        {
            a++;
        }
        else if(x == 2)
        {
            b++;
        }
        else if(x == 3)
        {
            c++;
        }
    }
}
