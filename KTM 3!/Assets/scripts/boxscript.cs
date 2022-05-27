using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxscript : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    public bool inHole = false;
    public bool held = false;

    int movetime = 0;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (inHole)
        {
            sprite.enabled = false;
            box.enabled = false;
        }
        else
        {
            sprite.enabled = true;
            box.enabled = true;
        }
    }

    public bool CheckMove(Vector2 direction)
    {
        box.enabled = false;

        RaycastHit2D move = Physics2D.Raycast(transform.position, direction, 1f);

        if (move == true)
        {            
            if (move.transform.CompareTag("walls"))
            {
                box.enabled = true;
                return true;
            }
            else if (move.transform.CompareTag("box"))
            {
                box.enabled = true;
                return move.transform.GetComponent<boxscript>().CheckMove(direction);
            }
            else
            {
                box.enabled = true;
                return false;
            }
        }
        else
        {
            box.enabled = true;
            return false;
        }

    }

    public void push(Vector2 direction)
    {
        box.enabled = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f);

        if(hit == true)
        {
            if (hit.transform.CompareTag("box"))
            {
                hit.transform.GetComponent<boxscript>().push(direction);
            }
        }

        box.enabled = true;

        StartCoroutine(MoveRoutine(direction));
    }

    public void swingAdjacent(Vector2 directionfacing, Vector2 directionpush)
    {
        Vector3 targetposition = FindObjectOfType<playercontroller>().transform.position + new Vector3(directionfacing.x, directionfacing.y, 0);

        RaycastHit2D hit = Physics2D.Raycast(targetposition, new Vector2(1, 0), 0.1f);

        if(hit == true)
        {
            if (hit.transform.CompareTag("box"))
            {
                hit.transform.GetComponent<boxscript>().push(directionpush);
            }
        }

        //rb2d.position = targetposition;
    }

    public void swingLateral(Vector2 direction, int corner)
    {
        Vector2 checkposition = new Vector2(0,0);

        switch (corner)
        {
            case 1:
                { 
                    checkposition = new Vector2(1,1);
                    break;
                }
            case 2:
                {
                    checkposition = new Vector2(-1, 1);
                    break;
                }
            case 3:
                {
                    checkposition = new Vector2(-1, -1);
                    break;
                }
            case 4:
                {
                    checkposition = new Vector2(1, -1);
                    break;
                }
        }

        checkposition = FindObjectOfType<playercontroller>().transform.position + new Vector3(checkposition.x,checkposition.y,0);

        RaycastHit2D hit = Physics2D.Raycast(checkposition, checkposition, 0.1f);

        if(hit == true)
        {
            if (hit.transform.CompareTag("box"))
            {
                hit.transform.GetComponent<boxscript>().push(direction);
            }
        }
    }

    public void VisualSwing(bool left)
    {
        float goalangle = 0;

        if (left)
        {
            goalangle = 90;
        }
        else
        {
            goalangle = -90;
        }

        StartCoroutine(SwingRoutine(goalangle));
    }

    public void CheckFloor()
    {

        if (held)
        {
            inHole = false;
        }
        else
        {
            box.enabled = false;

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(rb2d.position.x + 0.3f, rb2d.position.y + 0.3f), new Vector2(1, 1), 0.1f);

            if (hit == true)
            {
                print(hit.transform.name);

                if (hit.transform.CompareTag("hole"))
                {
                    StartCoroutine(fall(12));
                }
                else 
                {
                    inHole = false;
                }
            }
            else
            {
                inHole = false;
            }

            box.enabled = true;
        }
    }

    public void Undo(Vector2 position, bool Held)
    {
        rb2d.position = position;
        held = Held;

        VisualUpdate(Held);

        box.enabled = false;

        CheckFloor();
    }

    void VisualUpdate(bool Held)
    {
        if (Held)
        {
            this.transform.localScale = new Vector2(.9f, .9f);
        }
        else
        {
            this.transform.localScale = new Vector2(1, 1);
        }
    }

    IEnumerator MoveRoutine(Vector2 dir)
    {
        movetime = FindObjectOfType<playercontroller>().movetime;

        int x = 0;

        while (x < movetime)
        {

            transform.Translate(new Vector2(dir.x / movetime, dir.y / movetime), Space.Self);
            x++;

            yield return new WaitForEndOfFrame();
        }

        CheckFloor();
    }

    IEnumerator SwingRoutine(float goal)
    {
        movetime = FindObjectOfType<playercontroller>().movetime;

        int x = 0;

        while (x < movetime)
        {

            transform.RotateAround(FindObjectOfType<playercontroller>().transform.position, Vector3.forward, goal / movetime);
            x++;

            yield return new WaitForEndOfFrame();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator fall(float length)
    {
        float x = 0;
        
        while (x < length)
        {
            x++;

            this.transform.localScale = new Vector2(1 - x*(1/length), 1 - x*(1/length));

            yield return new WaitForEndOfFrame();
        }

        inHole = true;
    }
}
