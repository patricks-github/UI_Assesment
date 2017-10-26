using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_radial : MonoBehaviour {

    public GameObject player;

    GameObject Radial;
    Transform RadialOptions;

    GameObject Item1;
    GameObject Item2;
    GameObject Item3;
    GameObject Item4;
    GameObject Item5;
    GameObject Item6;
    GameObject Item7;
    GameObject Item8;

    bool tabbed = false;

    Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
    Vector2 mousePos;
    Vector2 mouseOffset;

    Vector3 itemUpscale = new Vector3(1,1,1) * 1.1f;
    Vector3 itemReset = new Vector3(1, 1, 1);
    float maxStretch = 150;
    float angleOffset = 10.0f + 10.0f;
    float mouseDist;
    float rotationStrength = 75;
    int selectedItem;

    // Use this for initialization
    void Start () {
        Radial = transform.Find("RadialWeapons_Holder").gameObject;
        RadialOptions = Radial.transform.Find("Radial").transform.Find("RadialOptions").gameObject.transform;

        Item1 = RadialOptions.GetChild(0).gameObject;
        Item2 = RadialOptions.GetChild(1).gameObject;
        Item3 = RadialOptions.GetChild(2).gameObject;
        Item4 = RadialOptions.GetChild(3).gameObject;
        Item5 = RadialOptions.GetChild(4).gameObject;
        Item6 = RadialOptions.GetChild(5).gameObject;
        Item7 = RadialOptions.GetChild(6).gameObject;
        Item8 = RadialOptions.GetChild(7).gameObject;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (tabbed)
        {
            // Mouse Angle
            mousePos = Input.mousePosition;
            mouseOffset = (mousePos - screenCentre);
            
            AngleBreakdown();

            switchselectedItem();
            
            mouseDist = Vector3.Distance(screenCentre, mousePos);
            mouseDist = Mathf.Clamp(mouseDist, 0, maxStretch);

            itemUpscale = itemReset * (1.05f + (mouseDist / 1000));

            //transform.Rotate(mouseOffset / 5);

            Radial.GetComponent<RectTransform>().localEulerAngles = new Vector3(mouseOffset.y / rotationStrength, mouseOffset.x / rotationStrength, 0);

            //Debug.Log("Mouse Offset: " + mouseOffset);
            //Debug.Log("Item: " + selectedItem);
        }
	}

    public void Tab()
    {
        if (player.GetComponent<scr_movement>().controllable)
        {
            tabbed = false;
        }
        else
        {
            //RadialOptions.gameObject.SetActive(false);
            tabbed = true;
        }
    }

    void AngleBreakdown()
    {
        float angleRaw = Mathf.Atan2(mouseOffset.y, mouseOffset.x);
        float angle = angleRaw * 120;

        if (angle > 0)
        {
            if (angle < 50)
                selectedItem = 5;
            else
            {
                if (angle < 140)
                    selectedItem = 4;
                else
                {
                    if (angle < 240)
                        selectedItem = 3;
                    else
                    {
                        if (angle < 330)
                        {
                            selectedItem = 2;
                        }
                        else
                            selectedItem = 1;
                    }
                }
            }
        }
        else
        {
            if (angle > -50)
                selectedItem = 5;
            else
            {
                if (angle > -140)
                    selectedItem = 6;
                else
                {
                    if (angle > -240)
                        selectedItem = 7;
                    else
                    {
                        if (angle > -330)
                        {
                            selectedItem = 8;
                        }
                        else
                            selectedItem = 1;
                    }
                }
            }
        }

        //Debug.Log("selectedItem: " + selectedItem + "   Angle: " + angle);
    }

    void switchselectedItem()
    {
        // Toggle item 1
        if (selectedItem == 1) {
            Item1.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            Item1.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 2
        if (selectedItem == 2) {
            Item2.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            Item2.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 3
        if (selectedItem == 3) {
            Item3.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            Item3.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 4
        if (selectedItem == 4) {
            Item4.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            Item4.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 5
        if (selectedItem == 5) {
            Item5.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            //Item5.SetActive(false);
            Item5.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 6
        if (selectedItem == 6) {
            Item6.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            //Item6.SetActive(false);
            Item6.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 7
        if (selectedItem == 7) {
            Item7.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            //Item7.SetActive(false);
            Item7.GetComponent<RectTransform>().localScale = itemReset;
        }

        // Toggle item 8
        if (selectedItem == 8) {
            Item8.GetComponent<RectTransform>().localScale = itemUpscale;
        }
        else {
            //Item8.SetActive(false);
            Item8.GetComponent<RectTransform>().localScale = itemReset;
        }
        
    }
}
