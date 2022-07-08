using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_script : MonoBehaviour
{
    public List<string> completelevels;
    public Vector2 mostrecentlevel;

    void Awake()
    {
        if (FindObjectsOfType<data_script>().Length != 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void WonLevel(int level)
    {
        completelevels.Add(level.ToString());
    }

    public void ChangeLastTouchedLevel(GameObject node)
    {
        mostrecentlevel = node.transform.position;
    }

    public void NewGame()
    {
        if(completelevels != null)
        {
            completelevels.Clear();
        }

        mostrecentlevel = new Vector2(0,17.25f);
    }
}
