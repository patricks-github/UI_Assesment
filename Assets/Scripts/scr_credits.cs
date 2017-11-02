using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_credits : MonoBehaviour {

    public GameObject Fade;
    Animator fadeAnim;

    bool exit = false;

    bool timer = false;

    float buttonTimer = 2.0f;
    float buttonTimerReset = 2.0f;

    // Use this for initialization
    void Start () {
        fadeAnim = Fade.GetComponent<Animator>();

        fadeAnim.SetTrigger("FadeOut");
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fadeAnim.SetFloat("Speed", 1);

            timer = true;

            if (!exit)
            {
                fadeAnim.SetTrigger("FadeIn");
                exit = true;
            }
        }
        else
        {

            if (timer)
            {
                buttonTimer -= 1 * Time.deltaTime;

                if (buttonTimer < 0)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}
