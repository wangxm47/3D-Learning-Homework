using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript2 : MonoBehaviour
{
    public const float g = 9.8f;//重力加速度

    public GameObject target;//终点
    public Transform temp;
    public float speed = 10;//水平初速度
    private float verticalSpeed;//上升方向初速度
    void Start()
    {
        target = GameObject.Find("empty");//终点
        float tmepDistance = Vector3.Distance(transform.position, target.transform.position);//水平距离
        float tempTime = tmepDistance / speed;//水平运动时间
        float riseTime, downTime;//上升时间，下降时间
        riseTime = downTime = tempTime / 2;//上升时间=下降时间=水平运动时间/2
        verticalSpeed = g * riseTime;//上升方向初速度=重力加速度*上升时间
        temp = this.transform;
    }
    private float time;
    void Update()
    {
        if (transform.position.y < target.transform.position.y)
        {
            //到达终点  
            return;
        }
        time += Time.deltaTime;
        float test = verticalSpeed - g * time;//上升中速度变化
        temp.position += Vector3.right * speed * Time.deltaTime;
        temp.position += Vector3.up * test * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, temp.position,10);
    }
}
