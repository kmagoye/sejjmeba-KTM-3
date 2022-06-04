using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodescript : MonoBehaviour
{
    public GameObject Node1;
    public GameObject Node2;
    public GameObject Node3;
    public List<nodescript> nodes;

    public bool Accesible = true;

    private void Start()
    {
        nodes.Add(Node1.GetComponent<nodescript>());
        if(Node2 != null)
        {
          nodes.Add(Node2.GetComponent<nodescript>());
        }
        if (Node3 != null)
        {
            nodes.Add(Node3.GetComponent<nodescript>());
        }
    }
}
