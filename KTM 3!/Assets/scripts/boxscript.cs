using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxscript : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;
    Animator animator;

    public bool x = true;
    bool y = true;
    public bool inHole = false;
    public bool held = false;
    public bool Horizontal = false;
    public int layer = 7;

    int movetime = 0;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inHole)
        {
            box.enabled = false;
        }
        else
        {
            box.enabled = true;
        }

        if(x == true && y == true)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
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
        bool oldinhole = inHole;
        
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
                if (hit.transform.CompareTag("hole"))
                {
                    StartCoroutine(Fall());
                }
                else 
                {
                    inHole = false;
                    sprite.sortingOrder = 7;
                }
            }
            else
            {
                inHole = false;
                sprite.sortingOrder = 7;
            }

            box.enabled = true;
        }

        if(oldinhole && !inHole)
        {
            animator.SetTrigger("undo");
        }
    }

    public void Undo(Vector2 position, bool Held, bool Hor)
    {
        rb2d.position = position;
        held = Held;
        Horizontal = Hor;

        VisualUpdate(Held);

        box.enabled = false;

        CheckFloor();
    }

    public void VisualUpdate(bool Held)
    {
        StartCoroutine(Delay(Held));
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

        x = 0;
        
        while(x < 3)
        {
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
     
    IEnumerator Fall()
    {
        int x = 0;
        sprite.sortingOrder = 1;

        while (x < 10)
        {
            x++;
            yield return new WaitForEndOfFrame();
        }

        animator.SetTrigger("fall");
        inHole = true; 
    }

    IEnumerator Delay(bool Held)
    {
        float x = 0;

        while (x < 10)
        {
            x++;
            yield return new WaitForEndOfFrame();
        }

        if (Held)
        {
            y = false;
        }
        else
        {
            if (!inHole)
            {
                y = true;
            }
            if (Horizontal)
            {
                animator.SetBool("hor", true);
            }
            else
            {
                animator.SetBool("hor", false);
            }
        }
    }
}
