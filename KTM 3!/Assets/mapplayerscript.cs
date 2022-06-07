using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapplayerscript : MonoBehaviour
{
    public nodescript currentNode;
    Rigidbody2D Rigidbody2D;

    nodescript[] allNodes;
    public List<GameObject> enabledNodes;

    public GameObject topNode;
    public GameObject bottomNode;
    public GameObject leftNode;
    public GameObject rightNode;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.position = currentNode.transform.position;
        allNodes = FindObjectsOfType<nodescript>();
        NodeRefresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            move(rightNode);
        }
        if (Input.GetKeyDown("left"))
        {
            move(leftNode);
        }
        if (Input.GetKeyDown("down"))
        {
            move(bottomNode);
        }
        if (Input.GetKeyDown("up"))
        {
            move(topNode);
        }

    }

    void move(GameObject targetNode)
    {
        if (targetNode)
        {
            Rigidbody2D.transform.position = targetNode.transform.position;
            currentNode = targetNode.GetComponent<nodescript>();
            NodeRefresh();
        }
    }

    void NodeRefresh()
    {
        enabledNodes.Clear();
        leftNode = rightNode = bottomNode = topNode = null;

        foreach (nodescript node in allNodes)
        {
            node.enabled = false;
        }

        foreach (GameObject node in currentNode.nodes)
        {
            node.GetComponent<nodescript>().enabled = true;
            enabledNodes.Add(node);
        }

        float x = 0;

        //right
        foreach (GameObject node in enabledNodes)
        {
            bool correctside = true;

            float diff = node.transform.position.x - transform.position.x;

            if (diff < 0)
            {
                correctside = false;
            }

            if (!rightNode)
            {
                if (correctside)
                {
                    x = diff;
                    rightNode = node;
                }
            }
            else
            {
                if (x > diff && correctside)
                {
                    x = diff;
                    rightNode = node;
                }
            }
        }

        x = 0;

        //left
        foreach (GameObject node in enabledNodes)
        {
            bool correctside = true;

            float diff = transform.position.x - node.transform.position.x;

            if(diff < 0)
            {
                correctside = false;
            }

            if (!leftNode)
            {
                if (correctside)
                {
                    x = diff;
                    leftNode = node;
                }
            }
            else
            {
                if (x > diff && correctside)
                {
                    x = diff;
                    leftNode = node;
                }
            }
        }

        x = 0;

        //bottom
        foreach (GameObject node in enabledNodes)
        {
            bool correctside = true;

            float diff = transform.position.y - node.transform.position.y;

            if (diff < 0)
            {
                correctside = false;
            }

            if (!bottomNode)
            {
                if (correctside)
                {
                    x = diff;
                    bottomNode = node;
                }
            }
            else
            {
                if (x > diff && correctside)
                {
                    x = diff;
                    bottomNode = node;
                }
            }
        }

        x = 0;

        //top
        foreach (GameObject node in enabledNodes)
        {
            bool correctside = true;

            float diff = node.transform.position.y - transform.position.y;

            if (diff < 0)
            {
                correctside = false;
            }

            if (!topNode)
            {
                if (correctside)
                {
                    x = diff;
                    topNode = node;
                }
            }
            else
            {
                if (x > diff && correctside)
                {
                    x = diff;
                    topNode = node;
                }
            }
        }
    }
}
