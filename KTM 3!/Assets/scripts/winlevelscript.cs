using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winlevelscript : MonoBehaviour
{
    Rigidbody2D rb2d;
    Camera Camera;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Camera = FindObjectOfType<Camera>();

        rb2d.transform.position = new Vector2(Camera.transform.position.x, Camera.transform.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
