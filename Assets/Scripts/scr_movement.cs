using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movement : MonoBehaviour {

    GameObject Head;
    Animator Anim;
    Vector3 movement_vector;
    public Vector3 display_movement;

    CharacterController player;

    public GameObject HUD;

    public bool controllable = true;
    bool inAir = false;
    public bool movement_Update;
    public bool paused;

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
    float stamina_drain = 50.0f;
    float stamina_regen = 5.0f;
    float jump_cost = 25.0f;

    float gravity = 9.81f;

    CursorLockMode cursorLockState;

    Vector2 mouseLook;
    Vector2 mouseSmooth;
    float smoothing = 2f;

    float viewRange = 85;
    public Vector2 sensitivity = new Vector2 (1.25f,1);

    // Use this for initialization
    void Start ()
    {
        stamina = stamina_max; 

        cursorLockState = CursorLockMode.Locked;
        CursorLockSet();

        Anim = GetComponent<Animator>();

        player = GetComponent<CharacterController>();
        Head = transform.Find("Head").gameObject;
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

        if (Input.GetKeyDown(KeyCode.Escape))
            Escape("Escape");

        if (Input.GetKeyDown(KeyCode.Tab))
            Escape("Tab");

        if (controllable == true)
        {
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
                        stamina += stamina_drain * Time.deltaTime;
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
        }
        else
        {
            //Stop directional movement if uncontrollable
            movement_modifier = 1;
            
            moveAxisForward = 0 * movement_speed * movement_modifier;
            moveAxisSide = 0 * movement_speed * movement_modifier;
        }

        //Apply movement
        moveAxisUp -= gravity * Time.deltaTime;

        movement_vector = new Vector3(moveAxisSide, moveAxisUp, moveAxisForward);
        movement_vector = transform.rotation * movement_vector;
        player.Move(movement_vector * Time.deltaTime);
    }

    public void Escape(string keyCode)
    {
        switch (controllable)
        {
            case (true):
                controllable = false;
                
                cursorLockState = CursorLockMode.None;
                CursorLockSet();
                break;

            case (false):
                controllable = true;
                
                cursorLockState = CursorLockMode.Locked;
                CursorLockSet();
                break;
        }

        switch (keyCode)
        {
            case ("Escape"):
                HUD.GetComponent<scr_hud>().Escape();
                break;
            case ("Tab"):
                HUD.GetComponent<scr_hud>().Escape();
                break;
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
}
