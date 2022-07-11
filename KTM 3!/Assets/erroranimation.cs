using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class erroranimation : MonoBehaviour
{
    public bool done = false;

    private void Start()
    { 
        FindObjectOfType<playercontroller>().canMove = false;
        FindObjectOfType<soundscript>().PlaySound("error");
    }

    private void Update()
    {
        if (done)
        {
            FindObjectOfType<playercontroller>().canMove = true;
            DestroyImmediate(gameObject);
        }
    }
}
