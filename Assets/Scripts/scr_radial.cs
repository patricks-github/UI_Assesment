using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_radial : MonoBehaviour {

    public GameObject player;

    GameObject Radial;
    Transform RadialOptions;
    Text itemTitle;
    Text itemDesc;

    GameObject Item1;
    GameObject Item2;
    GameObject Item3;
    GameObject Item4;
    GameObject Item5;
    GameObject Item6;
    GameObject Item7;
    GameObject Item8;

    string[] titleArray;
    string[] descArray;

    bool tabbed = false;

    Vector2 screenCentre;
    Vector2 mousePos;
    Vector2 mouseOffset;
    
    Vector3 itemUpscale = new Vector3(1,1,1) * 1;
    Vector3 itemReset = new Vector3(1, 1, 1);
    Vector3 itemHover = new Vector3(0,0,-50);
    Vector3 itemHoverReset = new Vector3(0, 0, 0);

    float minStretch = -150;
    float maxStretch = 200;
    float angleOffset = 10.0f + 10.0f;
    float mouseDist;
    float rotationStrength = 100;

    public int selectedItem = 1;
    int equppedItem = 0;

    GameObject SelectedGun;

    public Sprite[] GunSprite;

    // Use this for initialization
    void Start () {
        Radial = transform.Find("RadialWeapons_Holder").gameObject;
        RadialOptions = Radial.transform.Find("Radial").transform.Find("RadialOptions").gameObject.transform;
        itemTitle = Radial.transform.Find("Text").transform.Find("Item_Title").GetComponent<Text>();
        itemDesc = Radial.transform.Find("Text").transform.Find("Item_Description").GetComponent<Text>();

        screenCentre = new Vector2(Screen.width * (Radial.GetComponent<RectTransform>().anchorMin.x), Screen.height / 2);

        SelectedGun = transform.Find("HUD_Holder").transform.Find("HUD").transform.Find("Info_R").transform.Find("SelectedGun").gameObject;

        Item1 = RadialOptions.GetChild(0).gameObject;
        Item2 = RadialOptions.GetChild(1).gameObject;
        Item3 = RadialOptions.GetChild(2).gameObject;
        Item4 = RadialOptions.GetChild(3).gameObject;
        Item5 = RadialOptions.GetChild(4).gameObject;
        Item6 = RadialOptions.GetChild(5).gameObject;
        Item7 = RadialOptions.GetChild(6).gameObject;
        Item8 = RadialOptions.GetChild(7).gameObject;

        titleArray = new string[9];
        descArray = new string[9];

        titleArray[1] = "PISTOL";
        titleArray[2] = "SHOTGUN";
        titleArray[3] = "ASSAULT RIFLE";
        titleArray[4] = "SUBMACHINE GUN";
        titleArray[5] = "SNIPER RIFLE";
        titleArray[6] = "GRENADE LAUNCHER";
        titleArray[7] = "CHAIN GUN";
        titleArray[8] = "ROCKET LAUNCHER";

        descArray[1] = "SEMI-AUTOMATIC PISTOL";
        descArray[2] = "12 GAUGE SHORT RANGE SHOTGUN";
        descArray[3] = "BURST LONG DISTANCE RIFLE";
        descArray[4] = "FULLY AUTOMATIC SHORT DISTANCE SMG";
        descArray[5] = "HIGH POWER DISTANCE RIFLE";
        descArray[6] = "SEMI-AUTOMATIC FRAG LAUNCHER";
        descArray[7] = "MULTI-BARREL BARELY HAND HELD MACHINE GUN";
        descArray[8] = "LONG DISTANCE EXPLOSIVE LAUNCHER";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (tabbed)
        {
            //Debug.Log(titleArray[selectedItem]);

            itemTitle.text = titleArray[selectedItem];
            itemDesc.text = descArray[selectedItem];

            //Debug.Log("Mouse:  " + mousePos + "Screen Centre:  " +screenCentre + "Screen Centre: " + (Radial.GetComponent<RectTransform>().anchorMin.x));

            // Mouse Angle
            mousePos = Input.mousePosition;
            mouseOffset = (mousePos - screenCentre);
            
            AngleBreakdown();

            switchselectedItem();
            
            mouseDist = Vector3.Distance(screenCentre, mousePos);
            mouseDist = Mathf.Clamp(mouseDist, minStretch, maxStretch);

            itemUpscale = itemReset * (1.05f + (mouseDist / 1000));

            //transform.Rotate(mouseOffset / 5);

            Radial.GetComponent<RectTransform>().localEulerAngles = new Vector3(mouseOffset.y / rotationStrength, -mouseOffset.x / rotationStrength, 0);

            //Debug.Log("Mouse Offset: " + mouseOffset);
            //Debug.Log("Item: " + selectedItem);

            equppedItem = selectedItem;
        }
        else
        {
            //SelectedGun.SetActive(false);
            //SelectedGun.GetComponent<Image>().sprite = GunSprite[equppedItem];
            //Debug.Log("Sprite: " + equppedItem);
        }
	}

    public void Tab(bool holdingTab)
    {
        if (holdingTab)
        {
            tabbed = true;
        }
        else
        {
            tabbed = false;
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
                    if (angle < 270)
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
                    if (angle > -270)
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

        Debug.Log("Sprite:  " + selectedItem);
        //SelectedGun.GetComponent<Image>().sprite = GunSprite[equppedItem];
        //Debug.Log("selectedItem: " + selectedItem + "   Angle: " + angle);
    }

    void switchselectedItem()
    {
        SelectedGun.GetComponent<Image>().sprite = GunSprite[selectedItem];

        // Toggle item 1
        if (selectedItem == 1) {
            Item1.GetComponent<RectTransform>().localScale = itemUpscale;
            Item1.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            Item1.GetComponent<RectTransform>().localScale = itemReset;
            Item1.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 2
        if (selectedItem == 2) {
            Item2.GetComponent<RectTransform>().localScale = itemUpscale;
            Item2.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            Item2.GetComponent<RectTransform>().localScale = itemReset;
            Item2.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 3
        if (selectedItem == 3) {
            Item3.GetComponent<RectTransform>().localScale = itemUpscale;
            Item3.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            Item3.GetComponent<RectTransform>().localScale = itemReset;
            Item3.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 4
        if (selectedItem == 4) {
            Item4.GetComponent<RectTransform>().localScale = itemUpscale;
            Item4.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            Item4.GetComponent<RectTransform>().localScale = itemReset;
            Item4.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 5
        if (selectedItem == 5) {
            Item5.GetComponent<RectTransform>().localScale = itemUpscale;
            Item5.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            //Item5.SetActive(false);
            Item5.GetComponent<RectTransform>().localScale = itemReset;
            Item5.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 6
        if (selectedItem == 6) {
            Item6.GetComponent<RectTransform>().localScale = itemUpscale;
            Item6.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            //Item6.SetActive(false);
            Item6.GetComponent<RectTransform>().localScale = itemReset;
            Item6.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 7
        if (selectedItem == 7) {
            Item7.GetComponent<RectTransform>().localScale = itemUpscale;
            Item7.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            //Item7.SetActive(false);
            Item7.GetComponent<RectTransform>().localScale = itemReset;
            Item7.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }

        // Toggle item 8
        if (selectedItem == 8) {
            Item8.GetComponent<RectTransform>().localScale = itemUpscale;
            Item8.GetComponent<RectTransform>().localPosition = itemHover;
        }
        else {
            //Item8.SetActive(false);
            Item8.GetComponent<RectTransform>().localScale = itemReset;
            Item8.GetComponent<RectTransform>().localPosition = itemHoverReset;
        }
        
    }
}
