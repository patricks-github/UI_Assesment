using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camera : MonoBehaviour {

    GameObject Head;
    GameObject Camera;

    bool Interactable;

    float lookDistance = 1000.0f;

    // Use this for initialization
    void Start ()
    {
        Head = transform.Find("Head").gameObject;
        Camera = transform.Find("Head").transform.Find("Camera").gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Raycasting
        RaycastHit hitInfo;

        //Ray properties
        Ray rayCast = new Ray(Camera.transform.position, Camera.transform.forward * lookDistance);

        //Checking hit info
        if (Physics.Raycast(rayCast, out hitInfo))
        {
            Interactable = (hitInfo.collider.tag == "Interactable");
        }

        //Display ray in editor
        Debug.DrawRay(Camera.transform.position, Camera.transform.forward * hitInfo.distance);

        if (Interactable)
        {
            Debug.Log("Interactable: " + hitInfo.collider.name);
        }
        
    }
}
