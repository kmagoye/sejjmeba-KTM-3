using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalscript : MonoBehaviour
{
    public bool won = false;

    public Sprite on;
    public Sprite off;
    public SpriteRenderer SpriteRenderer;
    Rigidbody2D rb2d;

    int playerlayer = 1 << 12;

    public GameObject check;
    check_script MyCheck;
    public Vector2 offset;

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
    }

    private void Update()
    {
        MyCheck.On = won;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), .1f, playerlayer);

        if (hit)
        {
            won = true;
            SpriteRenderer.sprite = on;
        }
        else
        {
            won = false;
            SpriteRenderer.sprite = off;
        }
    }

    public void MoveChecks()
    {
        check_script[] checks = FindObjectsOfType<check_script>();
        float amnt = checks.Length;
        float dist = amnt - 1f;

        print(amnt);

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
