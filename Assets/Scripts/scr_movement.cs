using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movement : MonoBehaviour {

    GameObject Head;

    Vector3 movement_vector;

    CharacterController player;

    bool Controlable = true;

    float movement_speed = 2.0f;
    float jump_speed = 5.0f;
    float movement_modifier = 1;
    float moveAxisForward;
    float moveAxisSide;
    float moveAxisUp;

    float gravity = 9.81f;

    CursorLockMode cursorLockState;

    float rotX;
    float rotY;
    public Vector2 sensitivity = new Vector2 (1.25f,1);

    // Use this for initialization
    void Start ()
    {
        cursorLockState = CursorLockMode.Locked;
        CursorLockSet();

        player = GetComponent<CharacterController>();
        Head = transform.Find("Head").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Controlable == true)
        {
            //Mouse
            rotX = Input.GetAxis("Mouse X") * sensitivity.x;
            rotY = Input.GetAxis("Mouse Y") * -sensitivity.y;

            transform.Rotate(0, rotX, 0);
            Head.transform.Rotate(rotY, 0, 0);

            
            if (player.isGrounded)
            {
                moveAxisUp = 0;

                //Movement
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movement_modifier = 2;
                }
                else
                {
                    movement_modifier = 1;
                }

                if (Input.GetButton("Jump"))
                {
                    moveAxisUp = jump_speed;
                    Debug.Log("Jump");
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
}
