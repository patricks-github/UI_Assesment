using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movement : MonoBehaviour {

    GameObject Head;
    Animator Anim;
    Vector3 movement_vector;
    public Vector3 display_movement;

    CharacterController player;

    public GameObject Blur;
    public GameObject UI_Canvas;
    public GameObject Radial;
    public GameObject MetaMenuObject;
    GameObject HUD;

    public bool controllable = true;
    bool inAir = false;
    public bool movement_Update;
    public bool paused = false;

    public bool MetaMenuOpened = false;

    float movement_speed = 2.0f;
    float jump_speed = 5.0f;
    float movement_modifier = 1;
    float moveAxisForward;
    float moveAxisSide;
    float moveAxisUp;
    float timer = 0;
    float timer_reset = 0.05f;
    public float stamina;
    float stamina_max = 100.0f;
    float stamina_drain = 25.0f;
    float stamina_regen = 20.0f;
    float jump_cost = 10.0f;

    float gravity = 9.81f;

    CursorLockMode cursorLockState;

    KeyCode MetaMenu = KeyCode.Tab;
    KeyCode WeaponSelection = KeyCode.LeftAlt;

    Vector2 mouseLook;
    Vector2 mouseSmooth;
    float smoothing = 2f;

    float viewRange = 85;
    public Vector2 sensitivity = new Vector2 (1.25f,1);

    float lookDownAngle = 45;
    float headTurnSpeed = 5;

    Quaternion lookDown;

    // Use this for initialization
    void Start ()
    {
        stamina = stamina_max; 

        cursorLockState = CursorLockMode.Locked;
        CursorLockSet();

        Anim = GetComponent<Animator>();

        player = GetComponent<CharacterController>();
        Head = transform.Find("Head").gameObject;
        Blur = Head.transform.Find("Blur").gameObject;
        HUD = UI_Canvas.transform.Find("HUD_Holder").gameObject;

        Radial.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (movement_Update == false)
        {
            display_movement = new Vector3((transform.InverseTransformDirection(player.velocity).x), (transform.InverseTransformDirection(player.velocity).y), (transform.InverseTransformDirection(player.velocity).z));
        }
        else
        {
            if (timer > 0)
            {
                timer -= 1 * Time.deltaTime;
            }
            else
            {
                movement_Update = false;
            }
        }

        //Debug.Log("X = " + (int)(transform.InverseTransformDirection(player.velocity).x) + ", Y = " + (int)(transform.InverseTransformDirection(player.velocity).y) + ", Z = " + (int)(transform.InverseTransformDirection(player.velocity).z));

        //Pause Button
        if (Input.GetKeyDown(KeyCode.Escape))
            Menu("Esc");

        //Weapon Selection
        if (Input.GetKey(WeaponSelection))
            GunMenu("Open");
        else
            GunMenu("Close");

        //Meta Menu
        if (Input.GetKeyUp(MetaMenu))
        {
            Menu("Meta");
        }

        if (controllable == true)
        {
            if (Blur.activeSelf)
            {
                Blur.SetActive(false);
            }

            if (!HUD.activeSelf)
            {
                HUD.SetActive(true);
            }

            if (Radial.activeSelf)
            {
                Radial.SetActive(false);
            }

            //Mouse
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            mouseSmooth.x = Mathf.Lerp(mouseSmooth.x, md.x, 1f / smoothing);
            mouseSmooth.y = Mathf.Lerp(mouseSmooth.y, md.y, 1f / smoothing);

            mouseLook += mouseSmooth;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -viewRange, viewRange);

            Head.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);

            if (player.isGrounded)
            {
                //Movement
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (stamina > 0)
                    {
                        Anim.SetFloat("Sprint", 5.0f);
                        movement_modifier = 2;

                        stamina -= stamina_drain * Time.deltaTime;
                    }
                    else
                    {
                        movement_modifier = 1;
                    }
                }
                else
                {
                    Anim.SetFloat("Sprint", 1.0f);
                    movement_modifier = 1;

                    if (stamina < stamina_max)
                    {
                        stamina += stamina_regen * Time.deltaTime;
                    }
                }

                if (Input.GetButton("Jump") && stamina >= jump_cost)
                {
                    moveAxisUp = jump_speed;
                    stamina -= jump_cost;
                }

                if (inAir)
                {
                    Trigger_Land();
                    inAir = false;
                }

                if (Input.GetMouseButton(1))
                {
                    Anim.SetBool("Aiming", true);
                }
                else
                {
                    if (Anim.GetBool("Aiming") == true)
                    {
                        Anim.SetBool("Aiming", false);
                    }
                }
            }
            else
            {
                if (!inAir)
                {
                    Trigger_Jump();
                    inAir = true;
                }
            }

            //Movement Directional Control
            moveAxisForward = Input.GetAxis("Vertical") * movement_speed * movement_modifier;
            moveAxisSide = Input.GetAxis("Horizontal") * movement_speed * movement_modifier;
            moveAxisUp -= gravity * Time.deltaTime;
        }
        else
        {
            //Stop directional movement if uncontrollable
            movement_modifier = 1;
            if (moveAxisUp > 0)
            {
                moveAxisUp *= 0.85f;
            }
            moveAxisUp -= (gravity * 0.05f) * Time.deltaTime;

            moveAxisForward = 0 * movement_speed * movement_modifier;
            moveAxisSide = 0 * movement_speed * movement_modifier;

            if (Blur.activeSelf == false)
            {
                Blur.SetActive(true);
            }

            if (!paused && player.isGrounded)
                if (stamina < stamina_max)
                    stamina += (stamina_regen * 0.25f ) * Time.deltaTime;

            //Rotate downward when player isn't controllabe and game isn't paused (Tab key)
            if (!paused | MetaMenuOpened)
            {
                lookDown = Quaternion.Euler(lookDownAngle, Head.transform.eulerAngles.y, Head.transform.eulerAngles.z);

                if (Head.transform.rotation.x < lookDown.x)
                {
                    Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, lookDown, headTurnSpeed * Time.deltaTime);
                }
                else
                {
                    Head.transform.rotation = lookDown;
                }
            }
        }

        //Apply movement
        
        movement_vector = new Vector3(moveAxisSide, moveAxisUp, moveAxisForward);
        movement_vector = transform.rotation * movement_vector;
        player.Move(movement_vector * Time.deltaTime);

        if (paused)
        {
            if (MetaMenuOpened)
            {
                if (!MetaMenuObject.activeSelf)
                {
                    MetaMenuObject.SetActive(true);
                }

                if (!Blur.activeSelf)
                {
                    Blur.SetActive(true);
                }
            }
        }
        else
        {
            if (!MetaMenuOpened)
            {
                if (MetaMenuObject.activeSelf)
                {
                    MetaMenuObject.SetActive(false);
                }

                if (Blur.activeSelf)
                {
                    Blur.SetActive(false);
                }
            }
        }
    }

    public void GunMenu(string keyCode)
    {
        if (!paused)
        {
            switch (keyCode)
            {
                case ("Open"):
                    if (controllable)
                        cursorLockState = CursorLockMode.Locked;
                        CursorLockSet();

                    if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("ArmUp"))
                        Anim.SetTrigger("ArmUp");


                    UI_Canvas.GetComponent<scr_hud>().Tab();

                    controllable = false;

                    Radial.SetActive(true);

                    UI_Canvas.GetComponent<scr_radial>().Tab(true);

                    Cursor.visible = true;

                    cursorLockState = CursorLockMode.None;
                    CursorLockSet();

                    //Debug.Log("TAB");
                    break;

                case ("Close"):
                    if (!controllable)
                    {
                        UI_Canvas.GetComponent<scr_hud>().Tab();
                        UI_Canvas.GetComponent<scr_radial>().Tab(false);
                        Radial.SetActive(false);


                        Cursor.visible = false;
                        cursorLockState = CursorLockMode.Locked;
                        CursorLockSet();
                        
                        Anim.SetTrigger("ArmDown");
                    }

                    controllable = true;
                    
                    break;
            }
        }
    }

    public void Menu(string MenuType)
    {
        if (!paused)
        {
            Radial.SetActive(false);
            paused = true;
            controllable = false;
            UI_Canvas.GetComponent<scr_hud>().Escape();
            cursorLockState = CursorLockMode.None;
            Cursor.visible = true;

            switch (MenuType)
            {
                default:
                    break;

                case ("Esc"):
                    DebugText("Esc");
                    break;

                case ("Meta"):
                    MetaMenuOpened = true;
                    if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("ArmUp"))
                    {
                        Anim.SetTrigger("ArmUp");
                    }
                    break;
            }

            CursorLockSet();
        }
        else
        {
            controllable = true;

            UI_Canvas.GetComponent<scr_hud>().Escape();

            Cursor.visible = false;
            cursorLockState = CursorLockMode.Locked;
            CursorLockSet();

            switch (MenuType)
            {
                default:
                    break;

                case ("Esc"):
                    DebugText("Esc");
                    break;

                case ("Meta"):
                    DebugText("Meta");
                    if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("ArmDown"))
                    {
                        Anim.SetTrigger("ArmDown");
                    }
                    MetaMenuOpened = false;
                    break;
            }

            paused = false;
        }
        
    }

    public void CursorLockSet()
    {
        Cursor.lockState = cursorLockState;
    }

    public void Trigger_Jump()
    {
        //Debug.Log("Jump");

        Anim.SetTrigger("Jump");
        display_movement.y = 2;
        movement_Update = true;
        timer = timer_reset;
    }

    public void Trigger_Land()
    {
        //Debug.Log("Landed");

        Anim.SetTrigger("Land");
        display_movement.y = 2;
        movement_Update = true;
        timer = timer_reset;
    }

    public void Shoot()
    {
        //Debug.Log("Landed");
        movement_Update = true;
        timer = timer_reset / 2;

        if (Anim.GetBool("Aiming") == true)
        {
            Anim.SetTrigger("AimShoot");
        }
        else
        {
            Anim.SetTrigger("Shoot");
        }

        UI_Canvas.GetComponent<scr_hud>().shoot();
    }

    public void Reload()
    {
        Anim.SetTrigger("Reload");
    }

    void DebugText(string text)
    {
        Debug.Log(text);
    }
}
