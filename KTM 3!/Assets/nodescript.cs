using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nodescript : MonoBehaviour
{
    public GameObject leftNode;
    public GameObject rightNode;
    public GameObject upNode;
    public GameObject downNode;

    public Object level;

    public bool won = false;
    public bool playable = false;
    public bool firstlevel = false;
    public bool eitheror = false;

    public List<nodescript> precursors;

    public GameObject lightsobject;
    GameObject lights;
    SpriteRenderer sprite;

    private void Start()
    {
        lights = Instantiate(lightsobject, gameObject.transform);
        sprite = lights.GetComponent<SpriteRenderer>();
        
        if (firstlevel)
        {
            playable = true;
        }
    }

    private void Update()
    {
        foreach(string completelevel in FindObjectOfType<data_script>().completelevels)
        {
            if(completelevel == gameObject.name)
            {
                won = true;
            }
        }

        if (!eitheror)
        {
            int score = 0;

            foreach (nodescript precursor in precursors)
            {
                if (!precursor.won)
                {
                    score++;
                }
            }

            if (score == 0)
            {
                playable = true;
            }
        }
        else
        {
            foreach (nodescript precursor in precursors)
            {
                if (!precursor.won)
                {
                    playable = true;
                }
            }
        }

        if(won && playable)
        {
            sprite.sprite = lights.GetComponent<nodelightscript>().won;
        }
        else if (!won && playable)
        {
            sprite.sprite = lights.GetComponent<nodelightscript>().playable;
        }
        else
        {
            sprite.sprite = lights.GetComponent<nodelightscript>().off;
        }
    }
}
