using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonscript : MonoBehaviour
{
    SpriteRenderer sprite;
    public Sprite selected;
    public Sprite deselected;
    public bool Selected = false;
    public int type; //1, new game. 2, continue. 3, quit

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        if (Selected)
        {
            sprite.sprite = selected;
        }
        else
        {
            sprite.sprite = deselected;
        }

        Selected = false;
    }

    public void Click()
    {
        if(type == 1)
        {
            FindObjectOfType<data_script>().NewGame();
            FindObjectOfType<mapplayerscript>().GetComponent<Rigidbody2D>().position = new Vector2(0, 17.25f);
            FindObjectOfType<menuplayerscript>().MenutoMap(120f);
        }
        else if (type == 2)
        {
            FindObjectOfType<menuplayerscript>().MenutoMap(120f);
        }
        else if (type == 3)
        {
            Application.Quit();
        }
    }

}
