using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //创建一个共有的游戏物体子弹
    public GameObject bullet;
    //速度的大小
    public float speed = 10f;
    //命名的一个数
    private int ballIndex = 0;
   void Start()
    {
        //初始化数组
    }
    private void FixedUpdate()
    {
        Shoots();
    }

    public Vector3 targetPoint;

    public float timer = 0;

    public float rate = 5;

    private RaycastHit hit;


    private void Shoots()
    {
        if (Input.GetKey(KeyCode.Mouse0))//按下鼠标左键
        {
            timer += Time.deltaTime;//计时器计时
            if (timer > 1f / rate)//如果计时大于子弹的发射速率（rate每秒几颗子弹）
            {
                //通过摄像机在屏幕中心点位置发射一条射线
                Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(ray, out hit))//如果射线碰撞到物体
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green);
                }
                else//射线没有碰撞到目标点
                {
                    //将目标点设置在摄像机自身前方1000米处
                    targetPoint = GetComponent<Camera>().transform.forward * 1000;
                }


                ballIndex++;
                //克隆一个物体，位置是所挂脚本物体的位置
                GameObject ball = GameObject.Instantiate(bullet, transform.position, transform.rotation);
                //克隆物体的名字
                ball.name = "Ball" + ballIndex;
                //克隆物体加入数组
                Rigidbody rig = ball.GetComponent<Rigidbody>();
                // ball.transform.Rotate(90, 90, 0, Space.Self);
                //克隆物体的速度是所挂脚本向前的速度
                rig.velocity = ball.transform.forward * speed;
                // ball.AddForce(speed * ray.direction);
                //发射数来的球沿着摄像机到鼠标点击的方向进行移动
                // GameObject ball = GameObject.Instantiate(bullet, transform.position, transform.rotation);
                //在枪口的位置实例化一颗子弹，按子弹发射点出的旋转，进行旋转
                bullet.transform.LookAt(targetPoint);//子弹的Z轴朝向目标
                Destroy(ball, 10);//在10S后销毁子弹
                timer = 0;//时间清零
            }
        }
    }



}
