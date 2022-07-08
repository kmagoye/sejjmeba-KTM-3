using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuplayerscript : MonoBehaviour
{
    Rigidbody2D rb2d;
    Camera camera;
    public List<GameObject> mainmenu;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        rb2d.position = camera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(rb2d.position, rb2d.position, 0.1f);

        if (hit)
        {
            if (hit.transform.CompareTag("button"))
            {
                hit.transform.GetComponent<buttonscript>().Selected = true;

                if (Input.GetMouseButtonDown(0))
                {
                    hit.transform.GetComponent<buttonscript>().Click();
                }
            }
        }
    }

    public void MenutoMap(float length)
    {
        StartCoroutine(MenuFadeOut(length));
        this.GetComponent<menuplayerscript>().enabled = false;
        FindObjectOfType<mapplayerscript>().GetComponent<mapplayerscript>().InMap = true;
    }

    IEnumerator MenuFadeOut(float length)
    {
        float x = 0;

        while (x < length)
        {
            foreach(GameObject thing in mainmenu)
            {
                Color c = new Color(1f,1f,1f, (1f - x * 1f/length));

                thing.GetComponent<SpriteRenderer>().color = c;
            }

            x++;
            yield return new WaitForEndOfFrame();
        }

        FindObjectOfType<mapplayerscript>().GetComponent<mapplayerscript>().StartStuff();

        foreach (GameObject thing in mainmenu)
        {
            thing.GetComponent<SpriteRenderer>().enabled = false;
            
            if (thing.CompareTag("button"))
            {
                thing.GetComponent<buttonscript>().enabled = false;
            }
        }
    }
}
