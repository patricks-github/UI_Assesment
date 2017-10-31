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

    GameObject HUD;
    GameObject HUDHolder;
    GameObject Interactable;
    GameObject Compass;
    public GameObject Counter;
    Image CounterBar;
    Text CounterText;

    GameObject CrosshairL;
    GameObject CrosshairR;

    //Image SelectedGun;

    Image Stamina;

    RectTransform HUDPosition;
    
    Vector3 Displacement;
    float crosshairDisplacement;

    public float compassOffset;

    public float intensity;
    
    float shootingTimer = 0;
    float shootingTimerReset = 1f;
    float shootingTimerDelay = 0.1f;
    float shotStrength = 20f;

    // Use this for initialization
    void Start () {
        HUD = transform.Find("HUD_Holder").transform.Find("HUD").gameObject;
        HUDHolder = transform.Find("HUD_Holder").gameObject;
        //HitmarkerAnim = transform.Find("Hitmarker").GetComponent<Animator>();

        HUDAnim = transform.GetComponent<Animator>();

        Compass = HUD.transform.Find("BarBottom").transform.Find("Compass").gameObject;
        Interactable = HUD.transform.transform.Find("Interactable").gameObject;
        Stamina = HUD.transform.Find("BarBottom").transform.Find("Health").GetComponent<Image>();

        CrosshairL = transform.Find("Crosshair").transform.Find("Crosshair_L").gameObject;
        CrosshairR = transform.Find("Crosshair").transform.Find("Crosshair_R").gameObject;

        CounterBar = Counter.transform.Find("CounterCanvas").transform.Find("Bar").GetComponent<Image>();
        CounterText = Counter.transform.Find("CounterCanvas").transform.Find("Text").GetComponent<Text>();

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

        if (shootingTimer <= 0)
        {
            Displacement = playerMovement.display_movement * intensity;
        }
        else
        {
            Displacement.x = playerMovement.display_movement.x;
            Displacement.y = playerMovement.display_movement.y;

            shootingTimer -= 1 * Time.deltaTime;

            if (shootingTimer <= (shootingTimerReset - shootingTimerDelay))
            {
                Displacement.z = Mathf.Abs(Displacement.z - 0.5f);
            }
        }
        

        //Hud movement
        HUDPosition.localPosition = new Vector2(-Displacement.x, -Displacement.y);
        HUDPosition.localScale = new Vector3(1 + (Displacement.z / 1000), 1 + (Displacement.z / 1000), 1);

        //Crosshair 
        crosshairDisplacement = ((Mathf.Abs(Displacement.x) + Mathf.Abs(Displacement.y) + Mathf.Abs(Displacement.z)) / 3) * 10 - 2;

        //Debug.Log( (Mathf.Abs(Displacement.x) + Mathf.Abs(Displacement.y) + Mathf.Abs(Displacement.z)) / 3);

        CrosshairL.transform.localPosition = new Vector2(-crosshairDisplacement, 0);
        CrosshairR.transform.localPosition = new Vector2(crosshairDisplacement, 0);

        //Gun Text
        CounterText.text = (playerCamera.ammo + " | " + playerCamera.ammoMax);

        if (!playerCamera.reloading)
        {
            CounterBar.fillAmount = (playerCamera.ammo * (1 / playerCamera.ammoMax));
        }
        else
        {
            CounterBar.fillAmount = 1 - playerCamera.reloadTimer;
        }

        //Debug.Log(Displacement.x);

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

    public void shoot()
    {
        shootingTimer = shootingTimerReset;
        if (Displacement.z < shotStrength)
        {
            Displacement.z -= shotStrength;
        }
    }
}
