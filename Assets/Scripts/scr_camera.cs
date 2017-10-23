using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camera : MonoBehaviour {

    GameObject Head;
    GameObject Camera;

    bool canShoot = true;
    public bool Interactable;
    public bool Enemy;

    float shotTimer;
    float shotTimer_reset = 0.1f;
    float damage = 50.0f;

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
        //Shoot timer
        if (!canShoot)
        {
            canShoot = false;

            if (shotTimer > 0)
            {
                shotTimer -= 1 * Time.deltaTime;
            }
            else
            {
                canShoot = true;
                Debug.Log("Can Shoot");
            }
        }

        //Raycasting
        RaycastHit hitInfo;
        //Ray properties
        Ray rayCast = new Ray(Camera.transform.position, Camera.transform.forward * lookDistance);
        
        //Checking hit info
        if (Physics.Raycast(rayCast, out hitInfo))
        {
            //Display ray in editor
            Debug.DrawRay(Camera.transform.position, Camera.transform.forward * hitInfo.distance);

            switch (hitInfo.collider.tag)
            {
                default:
                    Interactable = false;
                    Enemy = false;
                    break;

                case ("Interactable"):
                    if (hitInfo.distance < 1.5f)
                    {
                        Interactable = true;
                    }
                    else
                    {
                        Interactable = false;
                    }
                    break;

                case ("Enemy"):
                    Interactable = false;
                    Enemy = true;
                    break;
            }
        }

        if (Input.GetMouseButton(0) && canShoot)
        {
            Debug.Log("Shoot");

            shotTimer = shotTimer_reset;
            canShoot = false;

            if (Physics.Raycast(rayCast, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.gameObject.GetComponent<scr_enemy>().TakeHit(damage);
                    Debug.Log("Hit Enemy");
                }
                else
                {
                    Debug.Log("No Hit");
                }
            }
        }
    }
}
