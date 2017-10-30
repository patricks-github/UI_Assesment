using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_radialMenu : MonoBehaviour {

    int currentMenu = 0;

    GameObject Menu;
    GameObject Settings;

    GameObject L_MenuSelection;
    GameObject L_Video;

    GameObject R_Settings;

    GameObject Seperator;

    //Radial Stuff

    Vector2 screenCentre;
    Vector2 mousePos;
    Vector2 mouseOffset;

    float minStretch = -150;
    float maxStretch = 200;
    float angleOffset = 10.0f + 10.0f;
    float mouseDist;
    float rotationStrength = 100;

    float angle;

    bool Debug_hasClicked = false;
    float debug_fromAngle;
    float debug_toAngle;

    public int option;
    public Vector2 hoveredOption;

    Vector2 OptionMode_2x2 = new Vector2(2,2);
    Vector2 OptionMode_2x3 = new Vector2(2,3);
    Vector2 OptionMode_3x2 = new Vector2(3,2);
    Vector2 OptionMode_3x3 = new Vector2(3,3);

    // Use this for initialization
    void Start () {
        Menu = transform.Find("Menu").gameObject;
        Settings = transform.Find("SettingsMenu").gameObject;

        R_Settings = transform.Find("SettingsMenu").transform.Find("R_Settings").gameObject;

        L_MenuSelection = transform.Find("SettingsMenu").transform.Find("L_MenuSelection").gameObject;
        L_Video = transform.Find("SettingsMenu").transform.Find("L_VideoSettings").gameObject;

        Seperator = transform.Find("SettingsMenu").transform.Find("Seperator").gameObject;

        currentMenu = 0;

        Show(Menu);

        screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
    }
	
	// Update is called once per frame
	void Update () {

        mousePos = Input.mousePosition;
        mouseOffset = (mousePos - screenCentre);

        AngleBreakdown(OptionMode_2x2);

        mouseDist = Vector3.Distance(screenCentre, mousePos);
        mouseDist = Mathf.Clamp(mouseDist, minStretch, maxStretch);

        if (Input.GetMouseButtonUp(0))
            click();

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentMenu == 0)
        {
            currentMenu = 1;
            
            Show(Settings);

            Show(L_MenuSelection);
            Show(R_Settings);
            Show(Seperator);

            Hide(Menu);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentMenu == 1)
        {
            currentMenu = 0;

            Show(Menu);

            Hide(Settings);

            Hide(Seperator);
            Hide(L_MenuSelection);
            Hide(R_Settings);
        }
    }

    void Hide(GameObject HUDElement)
    {
        HUDElement.SetActive(false);
    }

    void Show(GameObject HUDElement)
    {
        HUDElement.SetActive(true);
    }

    void click()
    {
        if (!Debug_hasClicked)
        {
            //Debug.Log("From:  " + angle);
            Debug_hasClicked = true;
            debug_fromAngle = angle;
        }
        else
        {
            Debug_hasClicked = false;
            debug_toAngle = angle;
            Debug.Log("From" + debug_fromAngle + "  To:  " + debug_toAngle);
        }
    }

    void AngleBreakdown(Vector2 OptionCount)
    {
        float angleRaw = Mathf.Atan2(mouseOffset.y, mouseOffset.x);
        angle = angleRaw * 120;

        //Overly complicated angle breakdown checking the different amounts of options on each side and determining which one it's over.  I'm in too deep now really.
        if (angle > 0)
        {
            //R_Centre
            if (angle < 20)
            {
                //R_Top
                if ((OptionCount == OptionMode_2x2) | (OptionCount == OptionMode_3x2))
                {
                    //R_Top Modified If either 2 x 2 or 3 x 2 Configuration
                    //QuickDebug("R_Top Modified");

                    hoveredOption = new Vector2(0, 1);
                }
                else
                {
                    //R_Centre If in 2 x 3 or 3 x 3 configuration
                    //QuickDebug("R_Centre");

                    hoveredOption = new Vector2(0, 2);
                }
            }
            else
            {
                if (angle < 70)
                {
                    //R_Top
                    //QuickDebug("R_Top");

                    hoveredOption = new Vector2(0, 1);
                }
                else
                {
                    if (angle < 300)
                    {
                        //Nothing
                        //QuickDebug("Nothing");


                    }
                    else
                    {
                        if (angle < 355)
                        {
                            //L_Top
                            //QuickDebug("L_Top");

                            hoveredOption = new Vector2(1, 0);
                        }
                        else
                        {
                            //L_Centre


                            if ((OptionCount == OptionMode_2x2) | (OptionCount == OptionMode_2x3))
                            {
                                //L_Top Modified If either 2 x 2 or 2 x 3 Configuration
                                //QuickDebug("L_Top Modified");

                                hoveredOption = new Vector2(1, 0);
                            }
                            else
                            {
                                //L_Centre If in 3 x 2 or 3 x 3 configuration
                                //QuickDebug("L_Centre");

                                hoveredOption = new Vector2(2, 0);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //R_Centre
            if (angle > -20)
            {
                if ((OptionCount == OptionMode_2x2) | (OptionCount == OptionMode_3x2))
                {
                    //R_Bottom Modified If either 2 x 2 or 3 x 2 Configuration
                    QuickDebug("R_Bottom Modified");
                    hoveredOption = new Vector2(0, 3);
                }
                else
                {
                    //R_Centre If in 2 x 3 or 3 x 3 configuration
                    QuickDebug("R_Centre");
                    hoveredOption = new Vector2(0, 2);
                }
            }
            else
            {
                if (angle > -70)
                {
                    //R_Top
                    QuickDebug("R_Bottom");
                    hoveredOption = new Vector2(0, 3);
                }
                else
                {
                    if (angle > -300)
                    {
                        //Nothing
                        QuickDebug("Nothing");
                        hoveredOption = new Vector2(0, 0);
                    }
                    else
                    {
                        if (angle > -355)
                        {
                            //L_Top
                            QuickDebug("L_Bottom");
                            hoveredOption = new Vector2(3, 0);
                        }
                        else
                        {
                            //L_Centre
                            if ((OptionCount == OptionMode_2x2) | (OptionCount == OptionMode_2x3))
                            {
                                //L_Bottom Modified If either 2 x 2 or 2 x 3 Configuration
                                QuickDebug("L_Bottom Modified");
                                hoveredOption = new Vector2(3, 0);
                            }
                            else
                            {
                                //L_Centre If in 3 x 2 or 3 x 3 configuration
                                QuickDebug("L_Centre");
                                hoveredOption = new Vector2(2, 0);
                            }
                        }
                    }
                }
            }
        }
        //
    }

    void QuickDebug(string text)
    {
        //Debug.Log(text + "Mouse: " + angle);
        //Debug.Log(mouseDist);
    }
}
