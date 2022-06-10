﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playercontroller : MonoBehaviour
{
    public Vector2 directionfacing;

    public boxscript Box;
    public GameObject winscreen;
    public GameObject failmap;

    Rigidbody2D rb2d;
    undomanager Undomanager;
    failmapscript Failmap;
    controlmanagerscript Controls;
    pressureplatescript[] plates;
    Camera camera;
    Animator Animator;

    public Sprite right;
    public Sprite left;
    public Sprite up;
    public Sprite down;

    public float camwidth;
    public float camheight;

    int boxlayer = 1 << 9;
    int wallholelayer = 1 << 10;
    int playerlayer = 1 << 12;

    public int movetime;
    int fps;
    public int speed;

    public bool canMove = true;

    public bool startHolding = false;


    public bool lastlevel = false;

    bool testmode = true;

    public int lastmove = 0; //1,up 2,down 3,left, 4,right 5,space

    private void Start()
    {
        Instantiate(failmap);

        Application.targetFrameRate = 60;
        Controls = FindObjectOfType<controlmanagerscript>();
        plates = FindObjectsOfType<pressureplatescript>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Undomanager = GetComponent<undomanager>();
        Failmap = FindObjectOfType<failmapscript>().GetComponent<failmapscript>();
        camera = FindObjectOfType<Camera>();
        Animator = GetComponent<Animator>();

        StartCoroutine(FrameDelay(10));

        VisualUpdateInstant();
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown("up") && Controls.up == true)
            {
                Move(directionfacing, true);
                lastmove = 1;
            }
            if (Input.GetKeyDown("down") && Controls.down == true)
            {
                Move(directionfacing, false);
                lastmove = 2;
            }

            if (Input.GetKeyDown("right") && Controls.right == true)
            {
                CheckSwing(false);
            }
            if (Input.GetKeyDown("left") && Controls.left == true)
            {
                CheckSwing(true);
            }

            if (Input.GetKeyDown("space") && Controls.space)
            {
                PickUp();
            }

            if (Input.GetKeyDown("x"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        CheckWin();
    }

    private void Move(Vector2 directionmoving, bool foreward)
    {
        Failmap.Playermove();

        if (foreward == false)
        {
            directionmoving = -directionmoving;
        }

        if(CheckMove(directionmoving, foreward) == false) 
        { 
            Undomanager.Set();

            if (Box != null)
            {
                Box.push(directionmoving);

                if(foreward == false)
                {
                    CheckBox(directionmoving);
                }
            }
            else 
            {
                CheckBox(directionmoving);
            }

            StartCoroutine(MoveRoutine(directionmoving));
        }
    }

    private void VisualUpdateInstant()
    {
        if(directionfacing.x > 0)
        {
            Animator.SetTrigger("right");
        }
        if(directionfacing.x < 0)
        {
            Animator.SetTrigger("left");
        }
        if (directionfacing.y > 0)
        {
            Animator.SetTrigger("up");
        }
        if (directionfacing.x < 0)
        {
            Animator.SetTrigger("down");
        }
    }

    void VisualUpdate(Vector2 oldDir, Vector2 newDir)
    {
        if(oldDir.x > 0)
        {
            if(newDir.y > 0)
            {
                Animator.SetTrigger("right to up");
            }
            if (newDir.y < 0)
            {
                Animator.SetTrigger("right to down");
            }
        }
        if (oldDir.x < 0)
        {
            if (newDir.y > 0)
            {
                Animator.SetTrigger("left to up");
            }
            if (newDir.y < 0)
            {
                Animator.SetTrigger("left to down");
            }
        }
        if (oldDir.y > 0)
        {
            if (newDir.x < 0)
            {
                Animator.SetTrigger("up to left");
            }
            if (newDir.x > 0)
            {
                Animator.SetTrigger("up to right");
            }
        }
        if (oldDir.y < 0)
        {
            if (newDir.x < 0)
            {
                Animator.SetTrigger("down to left");
            }
            if (newDir.x > 0)
            {
                Animator.SetTrigger("down to right");
            }
        }
    }

    public void CheckSwing(bool left)
    {
        if (Box == null)
        {
            swing(left);
        }
        else
        {
            bool wall = false;

            //diagonal

            int corner = 0;

            if (left == false)
            {
                if (directionfacing == new Vector2(0, 1))
                {
                    corner = 1;
                }
                else if (directionfacing == new Vector2(1, 0))
                {
                    corner = 4;
                }
                else if (directionfacing == new Vector2(0, -1))
                {
                    corner = 3;
                }
                else if (directionfacing == new Vector2(-1, 0))
                {
                    corner = 2;
                }
            }
            if (left == true)
            {
                if (directionfacing == new Vector2(0, 1))
                {
                    corner = 2;
                }
                else if (directionfacing == new Vector2(-1, 0))
                {
                    corner = 3;
                }
                else if (directionfacing == new Vector2(0, -1))
                {
                    corner = 4;
                }
                else if (directionfacing == new Vector2(1, 0))
                {
                    corner = 1;
                }
            }

            Vector2 startpos = new Vector2(0,0);
            Vector2 direction = new Vector2(0, 0);
            Vector2 leftdir = new Vector2(0, 0);
            Vector2 rightdir = new Vector2(0, 0);

            switch (corner)
            {
                case 1:
                    {
                        startpos = new Vector2(transform.position.x + 1, transform.position.y + 1);

                        direction = new Vector2(1, 1);

                        leftdir = new Vector2(0, 1);

                        rightdir = new Vector2(1, 0);

                        break; 
                    }
                case 2:
                    {
                        startpos = new Vector2(transform.position.x - 1, transform.position.y + 1);

                        direction = new Vector2(-1, 1);

                        leftdir = new Vector2(-1, 0);

                        rightdir = new Vector2(0, 1);

                        break;
                    }
                case 3:
                    {
                        startpos = new Vector2(transform.position.x - 1, transform.position.y - 1);

                        direction = new Vector2(-1, -1);

                        leftdir = new Vector2(0, -1);

                        rightdir = new Vector2(-1, 0);

                        break;
                    }
                case 4:
                    {
                        startpos = new Vector2(transform.position.x + 1, transform.position.y - 1);

                        direction = new Vector2(1, -1);

                        leftdir = new Vector2(1, 0);

                        rightdir = new Vector2(0, -1);

                        break;
                    }
            }

            RaycastHit2D diagonal = Physics2D.Raycast(startpos, direction, 0.1f, ~playerlayer);

            if (diagonal == true)
            {
                if (diagonal.transform.CompareTag("walls"))
                {
                    wall = true;
                }
                else if (diagonal.transform.CompareTag("box"))
                {
                    if (left == true)
                    {
                        wall = diagonal.transform.GetComponent<boxscript>().CheckMove(leftdir);
                    }
                    else
                    {
                        wall = diagonal.transform.GetComponent<boxscript>().CheckMove(rightdir);
                    }
                }
            }

            //adjacent

            if (directionfacing == new Vector2(1, 0))
            {
                if (left == true)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
            }
            if (directionfacing == new Vector2(0, 1))
            {
                if (left == true)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if(hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
            }
            if (directionfacing == new Vector2(-1, 0))
            {
                if (left == true)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
            }   
            if (directionfacing == new Vector2(0, -1))
            {
                if (left == true)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1f, ~playerlayer);

                    if (hit == true)
                    {
                        if (hit.transform.CompareTag("walls"))
                        {
                            wall = true;
                        }
                        else if (hit.transform.CompareTag("box"))
                        {
                            if (hit.transform.GetComponent<boxscript>().CheckMove(-directionfacing) == true)
                            {
                                wall = true;
                            }
                        }
                    }
                }
            }

            if (wall == false)
            {
                swing(left); 
            }
        }
    }

    public void swing(bool left)
    {
        Undomanager.Set();

        Failmap.Playermove();

        if (!left)
        {
            lastmove = 4;

            Vector2 oldDirection = directionfacing;
            Vector2 directionpush = new Vector2(0, 0);
            int corner = 0;

            if (directionfacing == new Vector2(0, 1))
            {
                directionfacing = new Vector2(1, 0);
                directionpush = new Vector2(-0, -1);
                corner = 1;
            }
            else if (directionfacing == new Vector2(1, 0))
            {
                directionfacing = new Vector2(0, -1);
                directionpush = new Vector2(-1, 0);
                corner = 4;
            }
            else if (directionfacing == new Vector2(0, -1))
            {
                directionfacing = new Vector2(-1, 0);
                directionpush = new Vector2(0, 1);
                corner = 3;
            }
            else if (directionfacing == new Vector2(-1, 0))
            {
                directionfacing = new Vector2(0, 1);
                directionpush = new Vector2(1, 0);
                corner = 2;
            }
            
            VisualUpdate(oldDirection , directionfacing);

            if (Box != null)
            {
                Box.swingAdjacent(directionfacing, directionpush);
                Box.swingLateral(directionfacing, corner);
                Box.VisualSwing(left);
                Box.Horizontal = !Box.Horizontal;
            }
        }
        if(left)
        {
            lastmove = 3;

            Vector2 oldDirection = directionfacing;
            Vector2 directionpush = new Vector2(0, 0);
            int corner = 0;

            if (directionfacing == new Vector2(0, 1))
            {
                directionfacing = new Vector2(-1, 0);
                directionpush = new Vector2(0, -1);
                corner = 2;

            }
            else if (directionfacing == new Vector2(-1, 0))
            {
                directionfacing = new Vector2(0, -1);
                directionpush = new Vector2(1, 0);
                corner = 3;
            }
            else if (directionfacing == new Vector2(0, -1))
            {
                directionfacing = new Vector2(1, 0);
                directionpush = new Vector2(0, 1);
                corner = 4;
            }
            else if (directionfacing == new Vector2(1, 0))
            {
                directionfacing = new Vector2(0, 1);
                directionpush = new Vector2(-1, 0);
                corner = 1;
            }

            VisualUpdate(oldDirection, directionfacing);

            if (Box != null)
            {
                Box.swingAdjacent(directionfacing, directionpush);
                Box.swingLateral(directionfacing, corner);
                Box.VisualSwing(left);
                Box.Horizontal = !Box.Horizontal;
            }
        }
    }

    void CheckBox(Vector2 directionmoving)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionmoving, 1f, boxlayer);

        if(hit == true)
        {
            if (hit.transform.CompareTag("box"))
            {
                hit.transform.GetComponent<boxscript>().push(directionmoving);
            }
        }
    }

    private bool CheckMove(Vector2 directionmoving, bool foreward)
    {

        float dist = 1f;

        if (Box && foreward)
        {
            dist = 2f;
            Box.GetComponent<BoxCollider2D>().enabled = false;
        }

        RaycastHit2D move = Physics2D.Raycast(transform.position, directionmoving, dist, ~playerlayer);


        if(Box)
        {
            Box.GetComponent<BoxCollider2D>().enabled = true;
        }


        if (move)
        {
            if (Box && foreward)
            {
                RaycastHit2D infrontOfBox = Physics2D.Raycast(new Vector2(transform.position.x + 2 * directionfacing.x, transform.position.y + 2 * directionfacing.y), directionfacing, 0.1f, wallholelayer);

                if (infrontOfBox)
                {
                    if (infrontOfBox.transform.CompareTag("hole"))
                    {
                        RaycastHit2D infrontofPlayer = Physics2D.Raycast(new Vector2(transform.position.x + 1 * directionfacing.x, transform.position.y + 1 * directionfacing.y), directionfacing, 0.1f, wallholelayer);

                        if (infrontofPlayer)
                        {
                            return infrontofPlayer.transform.CompareTag("hole");
                        }
                    }

                    return infrontOfBox.transform.CompareTag("walls");
                }
                else
                {
                    RaycastHit2D infrontofPlayer = Physics2D.Raycast(new Vector2(transform.position.x + 1 * directionfacing.x, transform.position.y + 1 * directionfacing.y), directionfacing, 0.1f, wallholelayer);

                    if (infrontofPlayer)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (move.transform.CompareTag("walls") ^ move.transform.CompareTag("hole"))
                {
                    return true;
                }
            }

            return move.transform.CompareTag("box") ? move.transform.GetComponent<boxscript>().CheckMove(directionmoving) : false;
        }
        else
        {
            return false;
        }
    }

    void PickUp()
    {
        Failmap.Playermove();

        lastmove = 5;

        if (Box)
        {
            Undomanager.Set();

            Box.held = false;
            Box.VisualUpdate(false);
            Box.CheckFloor();
            Box = null;
        }
        else
        {
            RaycastHit2D box = Physics2D.Raycast(transform.position, directionfacing, 1f, boxlayer);

            if (box)
            {   
                if (box.transform.CompareTag("box"))
                {
                    Undomanager.Set();

                    Box = box.transform.GetComponent<boxscript>();
                    Box.VisualUpdate(true);
                    Box.held = true;
                    Box.inHole = false;
                }
            }
        }
    }  

    void CheckWin()
    {
        if (FindObjectOfType<goalscript>().won)
        {
            int score = 0;

            foreach (pressureplatescript plate in plates)
            {
                score = score + 1;

                if (plate.pressed)
                {
                    score = score - 1;
                }
            }

            if (score == 0)
            {
                Win();
            }
        }
    }

    void Win()
    {
        Instantiate(winscreen);
        this.GetComponent<playercontroller>().enabled = false;
        DataInput(6, gameObject.name);
    }

    public void Undo(Vector2 position, Vector2 direction, boxscript box)
    {
        rb2d.position = position;
        directionfacing = direction;
        Box = box;

        VisualUpdateInstant();
    }

    IEnumerator MoveRoutine(Vector2 dir)
    {
        canMove = false;

        int x = 0;

        while(x < movetime)
        {

            transform.Translate(new Vector2(dir.x / movetime, dir.y / movetime), Space.Self);
            x++;

            yield return new WaitForEndOfFrame();
        }

        canMove = true;
    }

    IEnumerator FrameDelay(int x)
    {
        int y = 0;

        while(y < x)
        {
            y++;
            yield return new WaitForEndOfFrame();
        }

        print(":P");

        fps = Mathf.RoundToInt(1 / Time.deltaTime);
        movetime = fps / speed;

        if (startHolding)
        {
            RaycastHit2D box = Physics2D.Raycast(transform.position, directionfacing, 1f, boxlayer);

            if (box)
            {
                if (box.transform.CompareTag("box"))
                {
                    Box = box.transform.GetComponent<boxscript>();
                    Box.transform.localScale = new Vector3(.9f, .9f, 1);
                    Box.held = true;
                    Box.inHole = false;

                    Undomanager.Set();
                }
            }
        }

        canMove = true;
    }

    public void DataInput(int x, string name) //1 -> 5 are moves, 6 ends level
    {
        if (testmode == false)
        {
            FindObjectOfType<data_script>().LastMove(x, name);
            
            if(x == 6)
            {
                FindObjectOfType<data_script>().EndLevel();
            }
        }
    }
  }
