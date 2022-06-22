using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_script : MonoBehaviour
{
    public List<string> completelevels;
    public Vector2 mostrecentlevel;

    private void Start()
    {
        DontDestroyOnLoad(this);   
    }

    public void WonLevel(int level)
    {
        completelevels.Add(level.ToString());
    }

    public void ChangeLastTouchedLevel(GameObject node)
    {
        mostrecentlevel = node.transform.position;
    }
}
