using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_pause: MonoBehaviour
{

    public GameObject MenuHolder;

    public GameObject Title;
    public GameObject Menu;
    public GameObject SubMenu;
    public GameObject Settings;

    public GameObject S_Video;
    public GameObject S_Audio;
    public GameObject S_Controls;

    public GameObject player;

    public GameObject Fade;

    public int page;
    public Vector3 option;

    Vector2 MenuPos;
    Vector2 SubMenuPos;
    Vector2 SettingsPos;

    float optionHeight = 40;
    float optionOffset = 13;

    public bool hasStarted;

    bool timer = false;

    float buttonTimer = 2.0f;
    float buttonTimerReset = 2.0f;

    int button = 0;

    Animator pageAnim;
    Animator fadeAnim;

    // Use this for initialization
    void Start()
    {
        pageAnim = GetComponent<Animator>();
        fadeAnim = transform.Find("Fade").gameObject.GetComponent<Animator>();

        hasStarted = false;
        page = 0;
        option = new Vector3(0, 0, 0);

        Debug.Log("Started");

        MenuPos = Menu.transform.localPosition;
        SubMenuPos = SubMenu.transform.localPosition;
        SettingsPos = Settings.transform.localPosition;

        MenuPos.y = optionOffset;
        SubMenuPos.y = optionOffset;
        SettingsPos.y = optionOffset;

        Menu.transform.localPosition = MenuPos;
        SubMenu.transform.localPosition = SubMenuPos;
        Settings.transform.localPosition = SettingsPos;

        refreshOptions();
    }

    // Update is called once per frame
    void Update()
    {

        inputs();

        if (timer)
        {
            buttonTimer -= 1 * Time.deltaTime;

            if (buttonTimer <= 0)
            {
                switch (button)
                {
                    default:
                        break;

                    case (0):
                        StartButton();
                        break;
                    case (1):
                        ContinueButton();
                        break;
                    case (2):
                        QuitButton();
                        break;
                }
            }
        }
        else
        {
            if (Fade.activeSelf)
            {
                Fade.SetActive(false);
            }
        }
    }

    void inputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            horizontalMove("Left");
        if (Input.GetKeyDown(KeyCode.RightArrow))
            horizontalMove("Right");
        if (Input.GetKeyDown(KeyCode.UpArrow))
            verticalMove("Up");
        if (Input.GetKeyDown(KeyCode.DownArrow))
            verticalMove("Down");

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            optionOffset += 1;

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            optionOffset -= 1;

        if (Input.anyKey)
        {
            if (!hasStarted)
            {
                hasStarted = true;
                pageAnim.SetTrigger("start");
            }
        }
    }

    void horizontalMove(string dir)
    {
        if (dir == "Left")
        {
            if (page > 0)
            {
                page -= 1;
            }
        }

        if (dir == "Right")
        {
            if (option.x == 1)
            {
                if (page < 3)
                {
                    page += 1;
                }
            }
            else
            {
                if (page < 2)
                {
                    page += 1;
                }
            }
        }

        if (page == 2)
        {
            switch ((int)option.x)
            {
                default:
                    break;

                case (0):
                    //MenuButtons("Start");
                    Resume();
                    player.GetComponent<scr_movement>().Menu("Esc");
                    break;

                case (2):
                    MenuButtons("Quit");
                    break;

            }
        }

        if (page == 1)
        {
            timer = false;
            buttonTimer = buttonTimerReset;
        }

        pageAnim.SetInteger("page", page);
    }

    void verticalMove(string dir)
    {
        if (page > 0)
        {
            if (dir == "Up")
            {
                switch (page)
                {
                    default:
                        break;

                    case (1):
                        if (option.x > 0)
                        {
                            option.x -= 1;
                        }
                        break;

                    case (2):
                        if (option.y > 0)
                        {
                            option.y -= 1;
                        }

                        option.z = 0;
                        break;

                    case (3):
                        if (option.z > 0)
                        {
                            option.z -= 1;
                        }
                        break;
                }
            }

            if (dir == "Down")
            {
                switch (page)
                {
                    default:
                        break;

                    case (1):
                        if (option.x < 2)
                        {
                            option.x += 1;
                        }
                        break;

                    case (2):
                        if (option.y < 2)
                        {
                            option.y += 1;
                        }

                        option.z = 0;
                        break;

                    case (3):
                        if (option.z < 2)
                        {
                            option.z += 1;
                        }
                        break;
                }

                Debug.Log("option: " + option.x + option.y + option.z);
            }
        }

        refreshOptions();

        MenuPos.y = option.x * optionHeight + optionOffset;
        SubMenuPos.y = option.y * optionHeight + optionOffset;
        SettingsPos.y = option.z * optionHeight + optionOffset;

        Menu.transform.localPosition = MenuPos;
        SubMenu.transform.localPosition = SubMenuPos;
        Settings.transform.localPosition = SettingsPos;
    }

    void refreshOptions()
    {
        switch ((int)option.x)
        {
            default:
                SubMenu.SetActive(false);
                Settings.SetActive(false);
                break;

            case (0):
                SubMenu.SetActive(false);
                Settings.SetActive(false);
                break;

            case (3):
                SubMenu.SetActive(false);
                Settings.SetActive(false);
                break;

            case (1):
                SubMenu.SetActive(true);
                Settings.SetActive(true);
                break;
        }

        switch ((int)option.y)
        {
            default:
                S_Video.SetActive(false);
                S_Audio.SetActive(false);
                S_Controls.SetActive(false);
                break;

            case (0):
                S_Video.SetActive(true);
                S_Audio.SetActive(false);
                S_Controls.SetActive(false);
                break;

            case (1):
                S_Video.SetActive(false);
                S_Audio.SetActive(true);
                S_Controls.SetActive(false);
                break;

            case (2):
                S_Video.SetActive(false);
                S_Audio.SetActive(false);
                S_Controls.SetActive(true);
                break;
        }
    }

    public void MenuButtons(string buttonType)
    {
        switch (buttonType)
        {
            default:
                break;

            case ("Start"):
                button = 0;
                break;

            case ("Continue"):
                button = 1;
                break;

            case ("Quit"):
                button = 2;
                break;
        }
        Fade.SetActive(true);
        fadeAnim.SetFloat("Speed", 1);
        timer = true;
    }

    public void StartButton()
    {
        //SceneManager.LoadScene("GameLevel");
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        pageAnim.SetInteger("page", 0);
        page = 0;
        option = new Vector3(0, 0, 0);
        hasStarted = false;
    }
}
