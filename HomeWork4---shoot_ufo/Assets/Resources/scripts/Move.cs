using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : SSAction
{
    public SceneController sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
    public GameObject target;   //要到达的目标  
    public float speed;    //速度  
    private float distanceToTarget;   //两者之间的距离  
    float startX;
    float targetX;
    float targetY;
    float targetz;
    public override void Start()
    {
        speed = 5 + sceneControler.round * 3;//使速度随着轮数变化
        startX = 7 - Random.value * 14;//使发射位置随机在（-10,10）
        targetX =20- Random.value*40;
        targetz = 25+Random.value*5;
        targetY = -5;
        this.transform.position = new Vector3(startX, 0, -15);
        target = new GameObject();//创建终点
        target.transform.position = new Vector3(targetX, targetY, targetz);
        //计算两者之间的距离  
        distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
    }
    public static Move GetSSAction()
    {
        Move action = ScriptableObject.CreateInstance<Move>();
        return action;
    }
    public override void Update()
    {
        Vector3 targetPos = target.transform.position;

        //让始终它朝着目标  
        gameobject.transform.LookAt(targetPos);

        //计算弧线中的夹角  
        float angle = Mathf.Min(1, Vector3.Distance(gameobject.transform.position, targetPos) / distanceToTarget) * 45;
        gameobject.transform.rotation = gameobject.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
        float currentDist = Vector3.Distance(gameobject.transform.position, target.transform.position);
        gameobject.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
        if (this.transform.position == target.transform.position)
        {
            sceneControler.factory.cacheDisk(gameobject);
            Destroy(target);
            this.enable = false;
            this.destroy = true;
        }
    }
    public override void FixedUpdate() { }
}
