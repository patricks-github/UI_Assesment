using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_hud : MonoBehaviour {

    public GameObject player;
    public scr_movement playerMovement;
    public scr_camera playerCamera;

    Animator HUDAnim;
    Animator HitmarkerAnim;

    GameObject HUD;
    GameObject Interactable;

    Image Stamina;

    RectTransform HUDPosition;
    
    Vector3 Displacement;

    public float intensity;

    // Use this for initialization
    void Start () {
        HUD = transform.Find("Hud").gameObject;

        Interactable = HUD.transform.transform.Find("Interactable").gameObject;
        Stamina = HUD.transform.Find("Stamina").gameObject.GetComponent<Image>();
        HUDAnim = transform.GetComponent<Animator>();
        HitmarkerAnim = transform.Find("Hitmarker").GetComponent<Animator>();

        HUDPosition = HUD.GetComponent<RectTransform>();

        playerMovement = player.GetComponent<scr_movement>();
        playerCamera = player.GetComponent<scr_camera>();

        intensity = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Stamina.fillAmount = playerMovement.stamina / 100;

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
        HitmarkerAnim.SetTrigger("Hit");
    }

    public void Escape()
    {
        switch (playerMovement.controllable)
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
