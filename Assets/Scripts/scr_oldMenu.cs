using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_oldMenu : MonoBehaviour {

    public GameObject currentButton;

    public Font fntNormal;
    public Font fntBold;

    Vector2 screenCentre;
    Vector2 mousePos;

    float mouseDist;

    float minStretch = 0;
    float maxStretch = 200;

    // Use this for initialization
    void Start () {
        screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);

    }
	
	// Update is called once per frame
	void Update () {
        // Mouse Angle
        mousePos = Input.mousePosition;

        //Mouse Distance
        mouseDist = Vector2.Distance(screenCentre, mousePos);
        mouseDist = Mathf.Clamp(mouseDist, minStretch, maxStretch) / 150;

        if (!(currentButton == null))
        {
            currentButton.transform.localScale = new Vector3(1, 1, 1) * (1 + ((mouseDist) - 1));
        }

        QuickDebug("Mousedist: " + mouseDist);

    }

    public void ButtonHover(GameObject Button)
    {
        Button.transform.Find("Text").GetComponent<Text>().font = fntBold;
        currentButton = Button;
    }

    public void ButtonExit(GameObject Button)
    {
        currentButton.transform.localScale = new Vector3(1, 1, 1);
        currentButton = null;
        Button.transform.Find("Text").GetComponent<Text>().font = fntNormal;
    }

    public void ButtonClick(GameObject Button)
    {
        //Get the reference button's name and figure out what to do with it.
        switch (Button.name)
        {
            default:
                break;

            case ("Quit"):
                QuickDebug("Button: " + Button.name);
                break;

            case ("Settings"):
                QuickDebug("Button: " + Button.name);
                break;

            case ("Credits"):
                QuickDebug("Button: " + Button.name);
                break;

            case ("NewGame"):
                QuickDebug("Button: " + Button.name);
                break;

            case ("Continue"):
                QuickDebug("Button: " + Button.name);
                break;

            case ("LoadGame"):
                QuickDebug("Button: " + Button.name);
                break;
        }
    }

    void QuickDebug(string text)
    {
        Debug.Log(text);
    }
}
