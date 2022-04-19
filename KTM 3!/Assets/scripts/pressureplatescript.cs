using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressureplatescript : MonoBehaviour
{

    SpriteRenderer SpriteRenderer;
    Rigidbody2D rb2d;

    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    public Sprite space;
    public Sprite on;

    public int type; // up:1 down:2 left:3 right:4 space:5
    public bool pressed;
    public bool startOn;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        SpriteRenderer = GetComponent<SpriteRenderer>();
        switch (type)
        {
            case 1:
                {
                    SpriteRenderer.sprite = up;
                    break;
                }
            case 2:
                {
                    SpriteRenderer.sprite = down;
                    break;
                }
            case 3:
                {
                    SpriteRenderer.sprite = left;
                    break;
                }
            case 4:
                {
                    SpriteRenderer.sprite = right;
                    break;
                }
            case 5:
                {
                    SpriteRenderer.sprite = space;
                    break;
                }
        }
    }

    public bool SteppedOn()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), .1f);

        if(hit == true)
        {
            if (hit.transform.CompareTag("box"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        RaycastHit2D box = Physics2D.Raycast(transform.position, new Vector2(0, 1), 0.1f);

        if (box == true && box.transform.CompareTag("box"))
        {
            if(box.transform.GetComponent<boxscript>().held == false)
            {
                pressed = true;
            }
            else
            {
                pressed = false;
            }
        }
        else
        {
            pressed = false;
        }

        FindObjectOfType<controlmanagerscript>().Toggle(type, this);

        if (pressed)
        {
            SpriteRenderer.sprite = on;
        }
        else
        {
            switch (type)
            {
                case 1:
                    {
                        SpriteRenderer.sprite = up;
                        break;
                    }
                case 2:
                    {
                        SpriteRenderer.sprite = down;
                        break;
                    }
                case 3:
                    {
                        SpriteRenderer.sprite = left;
                        break;
                    }
                case 4:
                    {
                        SpriteRenderer.sprite = right;
                        break;
                    }
                case 5:
                    {
                        SpriteRenderer.sprite = space;
                        break;
                    }
            }
        }
    }
}