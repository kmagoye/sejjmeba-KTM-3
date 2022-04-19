using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlmanagerscript : MonoBehaviour
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool space;

    public GameObject Up;
    public GameObject Down;
    public GameObject Left;
    public GameObject Right;
    public GameObject Space;

    pressureplatescript[] pressureplates;

    private void Start()
    {
        VisualUpdate();

        pressureplates = FindObjectsOfType<pressureplatescript>();
    }

    public void Toggle(int Type, pressureplatescript pressureplate)
    {
        switch (Type)
        {
            case 1:
                {
                    up = pressureplate.startOn ? !pressureplate.pressed : pressureplate.pressed;
                    break;
                }
            case 2:
                {
                    down = pressureplate.startOn ? !pressureplate.pressed : pressureplate.pressed;
                    break;
                }
            case 3:
                {
                    left = pressureplate.startOn ? !pressureplate.pressed : pressureplate.pressed;
                    break;
                }
            case 4:
                {
                    right = pressureplate.startOn ? !pressureplate.pressed : pressureplate.pressed;
                    break;
                }
            case 5:
                {
                    space = pressureplate.startOn ? !pressureplate.pressed : pressureplate.pressed;
                    break;
                }
            }

        VisualUpdate();
    }

    private void VisualUpdate()
    {
        if(up == true)
        {
            Up.GetComponent<Animator>().SetBool("on", true);
        }
        else 
        {
            Up.GetComponent<Animator>().SetBool("on", false);
        }
        if (down == true)
        {
            Down.GetComponent<Animator>().SetBool("on", true);
        }
        else
        {
            Down.GetComponent<Animator>().SetBool("on", false);
        }
        if (left == true)
        {
            Left.GetComponent<Animator>().SetBool("on", true);
        }
        else
        {
            Left.GetComponent<Animator>().SetBool("on", false);
        }
        if (right == true)
        {
            Right.GetComponent<Animator>().SetBool("on", true);
        }
        else
        {
            Right.GetComponent<Animator>().SetBool("on", false);
        }

        Animator[] animators = Space.GetComponentsInChildren<Animator>();

        if(space == true)
        {
            foreach(Animator animator in animators)
            {
                animator.SetBool("on", true);
            }
        }
        else
        {
            foreach (Animator animator in animators)
            {
                animator.SetBool("on", false);
            }
        }
    }

}
