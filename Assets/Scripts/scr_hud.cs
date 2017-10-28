using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_hud : MonoBehaviour {

    public GameObject player;

    scr_movement playerMovement;
    scr_camera playerCamera;
    scr_radial radial;

    Animator HUDAnim;
    Animator HitmarkerAnim;

    GameObject HUD;
    GameObject HUDHolder;
    GameObject Interactable;
    GameObject Compass;

    Image SelectedGun;

    Image Stamina;

    RectTransform HUDPosition;
    
    Vector3 Displacement;

    public float compassOffset;

    public float intensity;

    // Use this for initialization
    void Start () {
        HUD = transform.Find("HUD_Holder").transform.Find("HUD").gameObject;
        HUDHolder = transform.Find("HUD_Holder").gameObject;
        //HitmarkerAnim = transform.Find("Hitmarker").GetComponent<Animator>();

        HUDAnim = transform.GetComponent<Animator>();

        Compass = HUD.transform.Find("BarBottom").transform.Find("Compass").gameObject;
        Interactable = HUD.transform.transform.Find("Interactable").gameObject;
        Stamina = HUD.transform.Find("BarBottom").transform.Find("Health").GetComponent<Image>();

        HUDPosition = HUD.GetComponent<RectTransform>();

        playerMovement = player.GetComponent<scr_movement>();
        playerCamera = player.GetComponent<scr_camera>();
        radial = GetComponent<scr_radial>();

        intensity = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //SelectedGun = radial.selectedItem;

        Stamina.fillAmount = playerMovement.stamina / 100;
        
        //Compass
        compassOffset = player.transform.localEulerAngles.y / 360;
        Compass.GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex", new Vector2(compassOffset,0));

        Color Coolcolour = Compass.GetComponent<Renderer>().material.color;
        Coolcolour.a = transform.Find("HUD_Holder").transform.GetComponent<CanvasGroup>().alpha;

        Compass.GetComponent<Renderer>().material.color = Coolcolour;

        Displacement = playerMovement.display_movement * intensity;

        HUDPosition.localPosition = new Vector2(-Displacement.x, -Displacement.y);
        HUDPosition.localScale = new Vector3(1 + (Displacement.z / 1000), 1 + (Displacement.z / 1000), 1);

        if (Input.GetKeyUp(KeyCode.PageUp))
            intensity += 0.5f;

        if (Input.GetKeyUp(KeyCode.PageDown))   
            intensity -= 0.5f;
        
        
        if (playerCamera.Interactable)
        {
            Interactable.SetActive(true);
        }
        else
        {
            Interactable.SetActive(false);
        }
    }

    public void HitMarker()
    {
        //HitmarkerAnim.SetTrigger("Hit");
    }

    public void Tab()
    {
        if (player.GetComponent<scr_movement>().controllable)
        {
            HUDAnim.SetTrigger("Close");

        }
        else
        {

            HUDAnim.SetTrigger("Open");
        }
    }

    public void Escape()
    {
        switch (player.GetComponent<scr_movement>().controllable)
        {
            case (true):
                HUDAnim.SetTrigger("Open");
                break;

            case (false):
                HUDAnim.SetTrigger("Close");
                break;
        }
    }
}
