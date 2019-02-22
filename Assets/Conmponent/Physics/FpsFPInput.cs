using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsFPInput : MonoBehaviour
{
    public bool LockCursor
    {
        get
        {
            return Cursor.lockState == CursorLockMode.Locked ? true : false;
        }

        set
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }

    }

    private PlayerController player;

    private FpsInput input;


    private void Awake() {
        LockCursor = true;
        player = this.GetComponent<PlayerController>();
        input = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FpsInput>();
    }

    

    void InitialInput()
    {

        player.inputMoveVector = new Vector2(input.GetAxis("Horizontal"), input.GetAxis("Vertical"));
        player.inputSmoothLook = new Vector2(input.GetAxisRaw("Mouse X"), input.GetAxisRaw("Mouse Y"));
        player.inputCrouch = input.GetButton("Crouch");
        player.inputJump = input.GetButton("Jump");
        player.inputSprint = input.GetButton("Sprint");
        player.inputFire = input.GetButton("Fire");
    }

    void Update()
    {
        InitialInput();
    }
}
