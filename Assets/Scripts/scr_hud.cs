using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_hud : MonoBehaviour {

    public GameObject player;
    public scr_movement playerScript;

    GameObject HUD;
    RectTransform HUDPosition;
    
    Vector3 Displacement;

    public float intensity;

    // Use this for initialization
    void Start () {
        HUD = transform.Find("Hud").gameObject;
        HUDPosition = HUD.GetComponent<RectTransform>();
        playerScript = player.GetComponent<scr_movement>();
        intensity = 5;
    }

    // Update is called once per frame
    void Update()
    {

        Displacement = playerScript.display_movement * intensity;

        HUDPosition.localPosition = new Vector2(-Displacement.x, -Displacement.y);
        HUDPosition.localScale = new Vector2(1 + (Displacement.z / 1000), 1 + (Displacement.z / 1000));

        if (Input.GetKeyUp(KeyCode.PageUp))
            intensity += 0.5f;

        if (Input.GetKeyUp(KeyCode.PageDown))
            intensity -= 0.5f;
    }
}
