using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class undomanager : MonoBehaviour
{

    playercontroller player;
    boxscript[] boxes;

    Stack stack;

    private void Start()
    {
        stack = new Stack();
        player = FindObjectOfType<playercontroller>();
        boxes = FindObjectsOfType<boxscript>();
    }

    public void Set()
    {
        stack.Push(new gamestate());
    }
    private void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            gamestate gotstate = stack.Pop();

            player.Undo(gotstate.PlayerPos, gotstate.PlayerDir, gotstate.HeldBox);

            for(int i = 0; i < gotstate.BoxPos.Count; i++)
            {
                boxes[i].Undo(gotstate.BoxPos[i], gotstate.BoxHeld[i]);
            }
        }
    }

    public class gamestate
    {
        public Vector2 PlayerPos { get; set; }
        public Vector2 PlayerDir { get; set; }
        public boxscript HeldBox { get; set; }
        public List<Vector2> BoxPos = new List<Vector2>();
        public List<bool> BoxHeld = new List<bool>();

        public gamestate()
        {
            PlayerPos = FindObjectOfType<playercontroller>().transform.position;
            PlayerDir = FindObjectOfType<playercontroller>().directionfacing;
            HeldBox = FindObjectOfType<playercontroller>().Box ? FindObjectOfType<playercontroller>().Box : null;

            boxscript[] boxes = FindObjectsOfType<boxscript>();
            foreach (boxscript box in boxes)
            {
                BoxPos.Add(box.transform.position);
                BoxHeld.Add(box.held);
            }
        }
    }

    public class Stack
    {
        static readonly int MAX = 1000;
        int top;
        gamestate[] stack = new gamestate[MAX];

        public Stack()
        {
            top = -1;
        }
        public bool Push(gamestate data)
        {
            if (top >= MAX)
            {
                return false;
            }
            else
            {
                stack[++top] = data;
                return true;
            }
        }

        public gamestate Pop()
        {
            if (top < 0)
            {
                return null;
            }
            else
            {
                gamestate value = stack[top--];
                return value;
            }
        }
    }
}
