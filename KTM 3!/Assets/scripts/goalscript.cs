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

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    private void Update()
    {
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
}
