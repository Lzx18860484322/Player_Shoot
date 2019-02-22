using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    None,
    Idle,
    Walk,
    Crouch,
    Jump,
    Run,
}



public class fps_PlayerController : MonoBehaviour
{

    private PlayerState state = PlayerState.None;

    public PlayerState State
    {
        get
        {
            if (runing)
                state = PlayerState.Run;
            else if (walking)
                state = PlayerState.Walk;
            else if (crouching)
                state = PlayerState.Crouch;
            else
                state = PlayerState.Idle;

            return state;
        }

    }

    Animator anim;


    public float sprintSpend = 10.0f;
    public float sprintJumpSpend = 8.0f;
    public float normalSpend = 6.0f;
    public float normalJumpSpend = 7.0f;
    public float crouchSpend = 2.0f;
    public float crouchJumpSpend = 5.0f;
    //跳高度
    public float crouchDeltaHeight = 0.5f;

    public float gravity = 20f;
    public float cameraMoveSpend = 8f;
    public AudioClip jumpAudio;

    private float spend;
    private float jumpSpend;
    private Transform mainCamera;
    private float standarCamHeight;
    private float crouchingCamHeight;
    private bool grounded = false;
    private bool walking = false;
    private bool crouching = false;
    private bool stioCrouching = false;
    private bool runing;
    private Vector3 normalControllerCenter = Vector3.zero;
    private float normalControllerHeight = 0f;
    private float timer = 0;
    private CharacterController controller;
    private AudioSource audioSource;
    private PlayerController player;


    private void Start()
    {
        crouching = false;
        walking = false;
        runing = false;

        spend = normalSpend;
        jumpSpend = normalJumpSpend;
        mainCamera = GameObject.FindGameObjectWithTag(Tags.mainCamera).transform;
        //相机高度
        standarCamHeight = mainCamera.localPosition.y;
        crouchingCamHeight = standarCamHeight - crouchDeltaHeight;

        audioSource = this.GetComponent<AudioSource>();
        player = this.GetComponent<PlayerController>();
        controller = this.GetComponent<CharacterController>();
        normalControllerCenter = controller.center;
        normalControllerHeight = controller.height;

        anim = GetComponent<Animator>();
    }






    private void CurrentSpend()
    {
        switch (State)
        {
            case PlayerState.Idle:
                spend = normalSpend;
                jumpSpend = normalJumpSpend;
                break;
            case PlayerState.Walk:
                spend = normalSpend;
                jumpSpend = normalJumpSpend;
                break;
            case PlayerState.Crouch:
                spend = crouchSpend;
                jumpSpend = crouchJumpSpend;
                break;
            case PlayerState.Run:
                spend = sprintSpend;
                jumpSpend = sprintJumpSpend;
                break;
        }

    }


    private void AudioManagement()
    {

        if (State == PlayerState.Walk)
        {
            audioSource.pitch = 1.0f;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else if (State == PlayerState.Run)
        {
            audioSource.pitch = 1.3f;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else
            audioSource.Stop();
    }

    // private void Camera
    private void UpdateCrouch()
    {

        if (crouching)
        {
            if (mainCamera.localPosition.y > crouchingCamHeight)
            {
                if (mainCamera.localPosition.y - (crouchDeltaHeight * Time.deltaTime * cameraMoveSpend) < crouchingCamHeight)
                {
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
                }
                else
                {
                    mainCamera.localPosition = new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpend, 0);
                }
            }
        }

        else
        {
            if (mainCamera.localPosition.y < standarCamHeight)
            {
                if (mainCamera.localPosition.y + crouchDeltaHeight * Time.deltaTime * cameraMoveSpend > standarCamHeight)
                {
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standarCamHeight, mainCamera.localPosition.z);
                }
                else
                {
                    mainCamera.localPosition = new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpend, 0);
                }
            }
            else
            {
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standarCamHeight, mainCamera.localPosition.z);
            }
        }
    }

    private Vector3 moveDirection = Vector3.zero;

    private void UpdateMove()
    {


        if (grounded)
        {
            moveDirection = new Vector3(player.inputMoveVector.x, 0, player.inputMoveVector.y);
            Animating (player.inputMoveVector.x, player.inputMoveVector.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= spend;
            if (player.inputJump)
            {
                moveDirection.y = jumpSpend;
                AudioSource.PlayClipAtPoint(jumpAudio, transform.position);
                CurrentSpend();
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        //跳跃是否到地面
        CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

        if (Mathf.Abs(moveDirection.x) > 0 && grounded || Mathf.Abs(moveDirection.z) > 0 && grounded)
        {
            if (player.inputSprint)
            {
                walking = false;
                runing = true;
                crouching = false;
            }
            else if (player.inputCrouch)
            {
                walking = false;
                runing = false;
                crouching = true;
            }
            else
            {
                walking = true;
                runing = false;
                crouching = false;
            }
        }
        else
        {
            if (walking)
            {
                walking = false;
            }
            if (runing)
            {
                runing = false;
            }
            if (player.inputCrouch)
            {
                crouching = true;
            }
            else
            {
                crouching = false;
            }
        }

        if (crouching)
        {
            controller.height = normalControllerHeight - crouchDeltaHeight;
            controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 2, 0);

        }
        else
        {
            controller.height = normalControllerHeight;
            controller.center = normalControllerCenter;
        }
        UpdateCrouch();
        CurrentSpend();

    }

    private void FixedUpdate()
    {
        UpdateMove();
        AudioManagement();
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }




}
