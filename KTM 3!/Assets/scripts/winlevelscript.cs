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
            FindObjectOfType<transitionscript>().FadeOut();
            FindObjectOfType<data_script>().WonLevel(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        float x = FindObjectOfType<transitionscript>().length;
        float y = 0;

        while(y < x)
        {
            y++;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("map");
    }
}
