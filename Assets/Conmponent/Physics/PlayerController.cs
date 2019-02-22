using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [HideInInspector]
    public Vector2 inputSmoothLook;
    [HideInInspector]
    public Vector2 inputMoveVector;
    [HideInInspector]
    public bool inputCrouch;
    [HideInInspector]
    public bool inputJump;
    [HideInInspector]
    public bool inputSprint;
    [HideInInspector]
    public bool inputFire;
    //  [HideIninspector]
    // public bool inputBullet;

    // Start is called before the first frame update
    public Texture texture;
    private void Start()
    {

    }

    void OnGUI()
    {
        //texture.width >> 1和(texture.height >>是右移一位，
        //相当于除以2。(x >> 1) 和 (x / 2) 的结果是一样的。
        //创建一个新的矩形
        Rect rect = new Rect(Input.mousePosition.x - (texture.width >> 1),//矩形的X轴坐标
            Screen.height - Input.mousePosition.y - (texture.height >> 1),//矩形的y轴的坐标
            texture.width,//矩形的宽
            texture.height);//矩形的高
        GUI.DrawTexture(rect, texture);//开始绘制
    }
}
