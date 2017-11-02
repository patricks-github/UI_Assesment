using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_splash : MonoBehaviour {

    bool timerOn = false;
    float timer = 5.0f;


	// Use this for initialization
	void Start () {
        timerOn = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (timerOn)
        {
            timer -= 1 * Time.deltaTime;

            if (timer < 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
	}
}
