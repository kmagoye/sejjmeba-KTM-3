using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapplayerscript : MonoBehaviour
{
    public nodescript currentNode;
    Rigidbody2D Rigidbody2D;
    int map = 1 << 5;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.position = FindObjectOfType<data_script>().mostrecentlevel;

        RaycastHit2D hit = Physics2D.Raycast(Rigidbody2D.position, transform.position, 0.1f, ~map);

        if (hit)
        {
            currentNode = hit.transform.GetComponent<nodescript>();
        }

        if (FindObjectOfType<transitionscript>())
        {
            FindObjectOfType<transitionscript>().FadeIn(2);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("right") && currentNode.rightNode)
        {
            move(currentNode.rightNode);
        }
        if (Input.GetKeyDown("left") && currentNode.leftNode)
        {
            move(currentNode.leftNode);
        }
        if (Input.GetKeyDown("down") && currentNode.downNode)
        {
            move(currentNode.downNode);
        }
        if (Input.GetKeyDown("up") && currentNode.upNode)
        {
            move(currentNode.upNode);
        }

        if (Input.GetKeyDown("space") && currentNode.playable)
        {
            if (FindObjectOfType<transitionscript>())
            {
                FindObjectOfType<transitionscript>().FadeOut();
                StartCoroutine(Delay());
            }
            else
            {
                SceneManager.LoadScene(currentNode.level.name);
            }
        }
    }

    void move(GameObject targetNode)
    {
        if (targetNode)
        {
            Rigidbody2D.transform.position = targetNode.transform.position;
            currentNode = targetNode.GetComponent<nodescript>();
            FindObjectOfType<data_script>().ChangeLastTouchedLevel(targetNode);
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

        SceneManager.LoadScene(currentNode.level.name);
    }
}
