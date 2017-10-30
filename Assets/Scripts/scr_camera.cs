using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camera : MonoBehaviour {

    GameObject Head;
    GameObject Camera;
    scr_movement playerMovement;

    Ray rayCast;
    RaycastHit hitInfo;

    public GameObject HUD;

    bool canShoot = true;
    public bool Interactable;
    public bool Enemy;

    float shotTimer;
    float shotTimer_reset = 0.5f;
    float damage = 50.0f;

    float lookDistance = 1000.0f;

    public float ammo = 12;
    public float ammoMax = 12;

    public float reloadTimer;
    float reloadTimerReset = 1;

    public bool reloading;

    Vector3 lookDownRotation = new Vector3(-45, 0, 0);

    // Use this for initialization
    void Start ()
    {
        Head = transform.Find("Head").gameObject;
        Camera = transform.Find("Head").transform.Find("Camera").gameObject;

        playerMovement = GetComponent<scr_movement>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.GetComponent<scr_movement>().controllable)
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
                    //Debug.Log("Can Shoot");
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

            if (ammo > 0)
            {
                if (Input.GetMouseButton(0) && canShoot)
                {
                    Shoot();
                    ammo -= 1;
                }
            }
            
            if (Input.GetKey(KeyCode.R) && !reloading)
            {
                reloadTimer = reloadTimerReset;
                reloading = true;
                playerMovement.Reload();
            }
                

            if (reloading)
            {
                Debug.Log("Reloading");
                if (reloadTimer > 0)
                {
                    reloadTimer -= 1 * Time.deltaTime;
                }
                else
                {
                    reloadTimer = 0;
                    ammo = ammoMax;
                    reloading = false;
                }
            }
        }
        else
        {

        }

    }

    void Shoot()
    {
        Ray rayCast = new Ray(Camera.transform.position, Camera.transform.forward * lookDistance);

        //Debug.Log("Shoot");

        shotTimer = shotTimer_reset;
        canShoot = false;

        gameObject.GetComponent<scr_movement>().Shoot();

        if (Physics.Raycast(rayCast, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.gameObject.GetComponent<scr_enemy>().TakeHit(damage);
                //Debug.Log("Hit Enemy");

                HUD.GetComponent<scr_hud>().HitMarker();
            }
            else
            {
                //Debug.Log("No Hit");
            }
        }
    }
}
