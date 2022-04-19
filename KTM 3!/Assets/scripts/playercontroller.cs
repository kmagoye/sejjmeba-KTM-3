using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playercontroller : MonoBehaviour
{
    public Vector2 directionfacing;

    public boxscript Box;
    public GameObject SpriteRenderer;
    public GameObject winscreen;

    Rigidbody2D rb2d;
    undomanager Undomanager;
    controlmanagerscript Controls;
    pressureplatescript[] plates;
    Camera camera;

    public float camwidth;
    public float camheight;

    int boxlayer = 1 << 9;
    int wallholelayer = 1 << 10;
    int playerlayer = 1 << 12;

    public int movetime;
    int fps;
    public int speed;

    public bool startHolding = false;

    bool canMove = false;

    public bool lastlevel = false;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Controls = FindObjectOfType<controlmanagerscript>();
        plates = FindObjectsOfType<pressureplatescript>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Undomanager = GetComponent<undomanager>();
        camera = FindObjectOfType<Camera>();

        //camera.rect = new Rect(1,1,camwidth,camheight);

        //float x = (100f - 100f / (Screen.width / camwidth)) / 100f;
        //float y = (100f - 100f / (Screen.height / camheight)) / 100f;
        //camera.rect = new Rect(x, y, 1, 1);
        

        StartCoroutine(FrameDelay(10));

        VisualUpdateOld();
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown("up") && Controls.up == true)
            {
                Move(directionfacing, true);
            }
            if (Input.GetKeyDown("down") && Controls.down == true)
            {
                Move(directionfacing, false);
            }

            if (Input.GetKeyDown("right") && Controls.right == true)
            {
                CheckSwing(false);
            }
            if (Input.GetKeyDown("left") && Controls.left == true)
            {
                CheckSwing(true);
            }

            if (Input.GetKeyDown("x"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        PickUp();

        CheckWin();
    }

    private void Move(Vector2 directionmoving, bool foreward)
    {
        if(foreward == false)
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

    private void VisualUpdateOld()
    {
        if(directionfacing == new Vector2(1, 0))
        {
            SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (directionfacing == new Vector2(-1, 0))
        {
            SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (directionfacing == new Vector2(0, 1))
        {
            SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (directionfacing == new Vector2(0, -1))
        {
            SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void VisualUpdateNew(bool left)
    {
        float goalangle = 0;

        if (left)
        {
            goalangle = 90;
        }
        else
        {
            goalangle = - 90;
        }

        StartCoroutine(SwingRoutine(goalangle));
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

        if(!left)
        {
            VisualUpdateNew(false);

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

            if (Box != null)
            {
                Box.swingAdjacent(directionfacing, directionpush);
                Box.swingLateral(directionfacing, corner);
                Box.VisualSwing(left);
            }
        }
        if(left)
        {
            VisualUpdateNew(true);

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

            if (Box != null)
            {
                Box.swingAdjacent(directionfacing, directionpush);
                Box.swingLateral(directionfacing, corner);
                Box.VisualSwing(left);
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
        if (Input.GetKeyDown("space") && Controls.space)
        {
            if (Box)
            {
                Undomanager.Set();

                Box.held = false;
                Box.transform.localScale = new Vector3(1,1,1);
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
                        Box.transform.localScale = new Vector3(.9f, .9f, 1);
                        Box.held = true;
                        Box.inHole = false;
                    }
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
    }

    public void Undo(Vector2 position, Vector2 direction, boxscript box)
    {
        rb2d.position = position;
        directionfacing = direction;
        Box = box;

        VisualUpdateOld();
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

    IEnumerator SwingRoutine(float goal)
    {
        canMove = false;

        int x = 0;

        while (x < movetime)
        {

            SpriteRenderer.transform.Rotate(new Vector3(0, 0, goal/movetime));
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

  }
