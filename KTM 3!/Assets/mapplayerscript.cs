using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapplayerscript : MonoBehaviour
{
    public nodescript CurrentNode;
    Rigidbody2D Rigidbody2D;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.position = CurrentNode.transform.position;
    }
}
