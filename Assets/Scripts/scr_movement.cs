using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movement : MonoBehaviour {

    GameObject Head;
    Animator Anim;
    Vector3 movement_vector;

    CharacterController player;

    bool Controlable = true;
    bool inAir = false;

    float movement_speed = 2.0f;
    float jump_speed = 5.0f;
    float movement_modifier = 1;
    float moveAxisForward;
    float moveAxisSide;
    float moveAxisUp;

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
        cursorLockState = CursorLockMode.Locked;
        CursorLockSet();

        Anim = GetComponent<Animator>();

        player = GetComponent<CharacterController>();
        Head = transform.Find("Head").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Controlable == true)
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
                moveAxisUp = 0;

                //Movement
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Anim.SetFloat("Sprint", 5.0f);
                    movement_modifier = 2;
                }
                else
                {
                    Anim.SetFloat("Sprint", 1.0f);
                    movement_modifier = 1;
                }

                if (Input.GetButton("Jump"))
                {
                    moveAxisUp = jump_speed;
                    Debug.Log("Jump");
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

            moveAxisForward = Input.GetAxis("Vertical") * movement_speed * movement_modifier;
            moveAxisSide = Input.GetAxis("Horizontal") * movement_speed * movement_modifier;

            moveAxisUp -= gravity * Time.deltaTime;

            movement_vector = new Vector3(moveAxisSide, moveAxisUp, moveAxisForward);
            movement_vector = transform.rotation * movement_vector;
            player.Move(movement_vector * Time.deltaTime);
        }
    }

    public void CursorLockSet()
    {
        Cursor.lockState = cursorLockState;
    }

    public void Trigger_Jump()
    {
        Anim.SetTrigger("Jump");
    }

    public void Trigger_Land()
    {
        Anim.SetTrigger("Land");
    }
}
