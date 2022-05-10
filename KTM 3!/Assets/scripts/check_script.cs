using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check_script : MonoBehaviour
{
    public Sprite on;
    public Sprite off;

    public SpriteRenderer sprite;

    public bool On;

    private void Update()
    {
        if (On)
        {
            sprite.sprite = on;
        }
        else
        {
            sprite.sprite = off;
        }
    }
}
