using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapplayerscript : MonoBehaviour
{
    public nodescript currentNode;
    Rigidbody2D Rigidbody2D;
    int map = 1 << 5;
    public bool InMap;

    private void Start()
    {
        FindObjectOfType<transitionscript>().FadeIn(2,false);

        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.position = FindObjectOfType<data_script>().mostrecentlevel;

        StartStuff();
    }

    public void StartStuff()
    {
        RaycastHit2D hit = Physics2D.Raycast(Rigidbody2D.position, transform.position, 0.1f, ~map);

        if (hit)
        {
            currentNode = hit.transform.GetComponent<nodescript>();
        }
    }

    private void Update()
    {
        if (!InMap)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        } 
        

        if (InMap)
        {
            GetComponent<SpriteRenderer>().enabled = true;

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
                FindObjectOfType<soundscript>().PlaySound("menu");

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

            if (Input.GetKeyDown("escape"))
            {
                MaptoMenu();
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
            FindObjectOfType<soundscript>().PlaySound("menu");
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

    void MaptoMenu()
    {
        InMap = false;
        FindObjectOfType<menuplayerscript>().GetComponent<menuplayerscript>().enabled = true;
        StartCoroutine(MenuFadeIn());
    }

    IEnumerator MenuFadeIn()
    {
        List<GameObject> Mainmenu = FindObjectOfType<menuplayerscript>().mainmenu;

        float x = 0;
        float length = 120;

        foreach (GameObject thing in Mainmenu)
        {
            thing.GetComponent<SpriteRenderer>().enabled = true;

            if (thing.CompareTag("button"))
            {
                thing.GetComponent<buttonscript>().enabled = true;
            }
        }

        while (x < length)
        {
            foreach (GameObject thing in Mainmenu)
            {
                Color c = new Color(1f, 1f, 1f, (0f + x * 1f / length));

                thing.GetComponent<SpriteRenderer>().color = c;
            }

            x++;
            yield return new WaitForEndOfFrame();
        }

    }
}
