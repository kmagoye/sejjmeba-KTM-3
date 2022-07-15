using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalscript : MonoBehaviour
{
    public bool won = false;

    public SpriteRenderer SpriteRenderer;
    Rigidbody2D rb2d;
    Animator Animator;

    int playerlayer = 1 << 12;

    public GameObject check;
    check_script MyCheck;
    public Vector2 offset;

    bool x = false;

    private void Awake()
    { 
        Vector3 checkoffset = new Vector3((FindObjectOfType<Camera>().transform.position.x + offset.x), (FindObjectOfType<Camera>().transform.position.y + offset.y), 0f);
        GameObject x = Instantiate(check, checkoffset, transform.rotation);
        MyCheck = x.GetComponent<check_script>();

        pressureplatescript[] plates = FindObjectsOfType<pressureplatescript>();

        foreach (pressureplatescript plate in plates)
        {
            plate.MakeCheck();
        }
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        MoveChecks();
        Animator = GetComponent<Animator>();


    }

    private void Update()
    {
        MyCheck.On = won;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), .1f, playerlayer);

        if (hit)
        {
            won = true;
            if (!x)
            {
                x = true;

            }
        }
        else
        {
            won = false;
            x = false;
        }

        Animator.SetBool("won", won);
    }

    public void MoveChecks()
    {
        check_script[] checks = FindObjectsOfType<check_script>();
        float amnt = checks.Length;
        float dist = amnt - 1f;

        foreach(check_script check in checks)
        {
            check.transform.Translate(dist, 0, 0);
            dist--;
        }

        foreach (check_script check in checks)
        {
            check.transform.Translate(-(amnt-1)/2, 0, 0);
        }
    }
}
