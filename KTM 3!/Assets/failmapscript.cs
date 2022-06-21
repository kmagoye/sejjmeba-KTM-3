using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failmapscript : MonoBehaviour
{
    bool ShowA = false;
    bool ShowB = false;
    public bool Show;
    public int length = 60;
    boxscript[] boxes;
    pressureplatescript[] plates;

    int PlateNum = 0;

    SpriteRenderer sprite;

    private void Start()
    {
        StartCoroutine(wait(length));

        sprite = GetComponent<SpriteRenderer>();

        boxes = FindObjectsOfType<boxscript>();
        plates = FindObjectsOfType<pressureplatescript>();

        foreach(pressureplatescript plate in plates)
        {
            PlateNum++;
        }
    }

    private void Update()
    {
        int LiveBoxNum = 0;

        foreach (boxscript box in boxes)
        {
            if (!box.GetComponent<boxscript>().inHole)
            {
                LiveBoxNum++;
            }
        }

        ShowB = LiveBoxNum < PlateNum;

        if (ShowB ^ ShowA)
        {
            Show = true;
        }

        if (!ShowB && !ShowA)
        {
            Show = false;
        }

        sprite.enabled = Show;
    }

    public void Playermove()
    {
        StopAllCoroutines();
        StartCoroutine(wait(length));
    }

    IEnumerator wait(int time)
    {
        ShowA = false;

        int x = time;

        while(x > 0)
        {
            x = x - 1;
            yield return new WaitForEndOfFrame();
        }
        if(x == 0)
        {
            ShowA = true;
        }
    }
}
