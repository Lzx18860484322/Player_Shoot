using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class FpCamera : MonoBehaviour
{
    public Vector2 mouseLookSensitivity = new Vector2(5, 5);


    public Vector2 rotatuibXLimit = new Vector2(87, -87);


    public Vector2 rotatuibYLimit = new Vector2(-360, 360);

    public Vector3 positionOffset = new Vector3(0, 2, -0.2f);

    private Vector2 currentMouseLook = Vector2.zero;

    private float x_Angle = 0;

    private float y_Angle = 0;

    private PlayerController player;

    private Transform m_Transform;

    private Transform gun_Transform;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();
        m_Transform = transform;
        m_Transform.localPosition = positionOffset;
    }

    private void GetMouseLook()
    {
        currentMouseLook.x = player.inputSmoothLook.x;
        currentMouseLook.y = player.inputSmoothLook.y;

        currentMouseLook.x *= mouseLookSensitivity.x;
        currentMouseLook.y *= mouseLookSensitivity.y;

        currentMouseLook.y *= -1;
    }

    private void Update()
    {
        UpdateInput();
    }


    private void LateUpdate()
    {
        Quaternion xQu = Quaternion.AngleAxis(y_Angle, Vector3.up);
        Quaternion yQu = Quaternion.AngleAxis(0, Vector3.left);
        m_Transform.parent.rotation = xQu * yQu;

        yQu = Quaternion.AngleAxis(-x_Angle, Vector3.left);
        m_Transform.rotation = xQu * yQu;
    }



    private void UpdateInput()
    {
        if (player.inputSmoothLook == Vector2.zero)
        {
            return;
        }
        GetMouseLook();
        y_Angle += currentMouseLook.x;
        x_Angle += currentMouseLook.y;

        y_Angle = y_Angle < -360 ? y_Angle += 360 : y_Angle;
        y_Angle = y_Angle > 360 ? y_Angle -= 360 : y_Angle;
        y_Angle = Mathf.Clamp(y_Angle, rotatuibYLimit.x, rotatuibYLimit.y);

        x_Angle = x_Angle < -360 ? x_Angle += 360 : x_Angle;
        x_Angle = x_Angle > 360 ? x_Angle -= 360 : x_Angle;
        x_Angle = Mathf.Clamp(x_Angle, -rotatuibXLimit.x, -rotatuibXLimit.y);

    }


}
